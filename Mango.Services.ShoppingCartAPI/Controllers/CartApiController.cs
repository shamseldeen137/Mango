using AutoMapper;
using Mango.MessageBus.IMessaging;
using Mango.MessageBus.Messaging;
using Mango.RabbitMQ.Messaging;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Models.Dto;
using Mango.Services.ShoppingCartAPI.Models.Entities;
using Mango.Services.ShoppingCartAPI.Services.IServices;
using Mango.Services.ShoppingCartAPI.Services.Product;
using Mango.Services.ShoppingCartAPI.Utility;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/Cart/")]
    [ApiController]
    public class CartApiController : ControllerBase
    {
        private readonly ResponseDto _response;
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;
        private readonly IPublishMessage _messageBus;
       // private readonly IMessagePublisher _messageRabbitMq;

        private readonly RabbitMqPublisher _rabbitMqPublisher;
        private readonly AzureServiceBusPublisher _messageBusPublisher;
        private readonly MessagePublishContext _messagePublishContext;
        private IConfiguration _configuration;
        private readonly IAiService _aiService;


        public CartApiController(AppDbContext appDbContext, IMapper mapper, IProductService productService, ICouponService couponService, IPublishMessage messageBus, IConfiguration configuration, RabbitMqPublisher rabbitMqPublisher, AzureServiceBusPublisher messageBusPublisher, MessagePublishContext messagePublishContext, IAiService aiService)
        {
            _response = new ResponseDto();

            this._db = appDbContext;
            this._mapper = mapper;
            this._productService = productService;
            this._couponService = couponService;
            _messageBus = messageBus;
            // _messageRabbitMq = messageRabbitMq;
            _configuration = configuration;
            _rabbitMqPublisher = rabbitMqPublisher;
            _messageBusPublisher = messageBusPublisher;
            _messagePublishContext = messagePublishContext;
            _aiService = aiService;
        }
        [HttpPost("ApplyCoupon")]
        public async Task<ResponseDto> ApplyCoupon([FromBody]CartDto cartDto)
        {
            try
            {
                var cartHeaderFromDb=await _db.CartHeaders.FirstAsync(a => a.UserId == cartDto.CartHeader.UserId);
                cartHeaderFromDb.CouponCode = cartDto.CartHeader.CouponCode;
                _db.CartHeaders.Update(cartHeaderFromDb);
               await _db.SaveChangesAsync();
                _response.Result = true;
                _response.IsSuccuess = true;
          
            }
            catch (Exception ex)
            {

                _response.IsSuccuess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
            [HttpPost("RemoveCoupon")]
        public async Task<ResponseDto> RemoveCoupon([FromBody]CartDto cartDto)
        {
            try
            {
                var cartHeaderFromDb=await _db.CartHeaders.FirstAsync(a => a.UserId == cartDto.CartHeader.UserId);
                cartHeaderFromDb.CouponCode = "";
                _db.CartHeaders.Update(cartHeaderFromDb);
               await _db.SaveChangesAsync();
                _response.Result = true;
                _response.IsSuccuess = true;

            }
            catch (Exception ex)
            {

                _response.IsSuccuess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert(CartDto cartDto)
        {
            try
            {
                var cartHeaderEntity = _db.CartHeaders.AsNoTracking().FirstOrDefault(a => a.UserId == cartDto.CartHeader.UserId);
                if (cartHeaderEntity is null)
                { //User Does Not Has A Cart then Create A Cart 
                    var cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                    _db.CartHeaders.Add(cartHeader);

                    await _db.SaveChangesAsync();
                    cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First())
                       );
                    await _db.SaveChangesAsync();

                }
                else
                {
                    //Item Not Exist in the cart=> Create Item
                    var CartDetailsEntity = _db.CartDetails.AsNoTracking().FirstOrDefault(
                          u => u.ProductId == cartDto.CartDetails.First().ProductId
                          && u.CartHeaderId == cartHeaderEntity.CartHeaderId
                          );
                    if (CartDetailsEntity is null)
                    {
                        cartDto.CartDetails.First().CartHeaderId = cartHeaderEntity.CartHeaderId;

                        _db.CartDetails.Add(_mapper.Map<CartDetails>(cartDto.CartDetails.First())
                          );
                        await _db.SaveChangesAsync();

                    }

                    else
                    {
                        //item Exist=> Increase Count
                        cartDto.CartDetails.First().Count += CartDetailsEntity.Count;
                        cartDto.CartDetails.First().CartHeaderId = CartDetailsEntity.CartHeaderId;
                        cartDto.CartDetails.First().CartDetailsId = CartDetailsEntity.CartDetailsId;
                        _db.CartDetails.Update(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));

                        await _db.SaveChangesAsync();
                    }
                }
                _response.Result = cartDto;
                _response.IsSuccuess = true;

            }
            catch (Exception ex)
            {

                _response.Result = "";
                _response.IsSuccuess = false;
                _response.Message = ex.Message;
            }
            return _response;

        } 
        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> CartDetails(string userId)
        {
            try
            {
                CartDto cart = new() {
                    CartHeader = _mapper.Map<CartHeaderDto>(_db.CartHeaders.AsNoTracking().FirstOrDefault(a => a.UserId == userId)),

                };
                if (cart.CartHeader!=null)
                {
                    cart.CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(_db.CartDetails.AsNoTracking().Where(a => a.CartHeaderId == cart.CartHeader.CartHeaderId));

                
                IEnumerable<ProductDto> products = await _productService.GetAllProductsAsync();
                foreach (var item in cart.CartDetails)
                {
                    item.Product=products.FirstOrDefault(p => p.ProductId == item.ProductId);

                    cart.CartHeader.CartTotal += item.Count * item.Product.Price;
                }
                if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
                {
                    CouponDto couponDto = await _couponService.GetCouponByCodeAsync(cart.CartHeader.CouponCode);


                    if (couponDto is not null&& cart.CartHeader.CartTotal>=couponDto.MinAmount)
                    {

                        cart.CartHeader.CartTotal -= couponDto.DiscountAmount;
                        cart.CartHeader.Discount = couponDto.DiscountAmount;
                    }
                }
                    _response.Result = cart;
                    _response.IsSuccuess = true;
                }
                else
                {
                    _response.Message = "Cart Not Found";

                }


            }
            catch (Exception ex)
            {

                _response.Result = "";
                _response.IsSuccuess = false;
                _response.Message = ex.Message;
            }
            return _response;

        } [HttpPost("RemoveCart")]
        
       [HttpGet("RemoveFromCart")]

        public async Task<ResponseDto> RemoveCart([FromBody]int cartDetailsId)
        {
            try
            {
                CartDetails cartDetails =_db.CartDetails.AsNoTracking().First(a=>a.CartDetailsId==cartDetailsId);
                _db.CartDetails.Remove(cartDetails);
                int totalCountOfCartItem = _db.CartDetails.Where(c=>c.CartHeaderId==cartDetails.CartHeaderId).Count();
                if (totalCountOfCartItem == 1) {
                    var cartHeaderToRemove =await _db.CartHeaders.FirstOrDefaultAsync(c => c.CartHeaderId == cartDetails.CartHeaderId);
                    _db.CartHeaders.Remove(cartHeaderToRemove);
                    
                }
                await _db.SaveChangesAsync();
                
                _response.Result = true;
                _response.IsSuccuess = true;

            }
            catch (Exception ex)
            {

                _response.Result = "";
                _response.IsSuccuess = false;
                _response.Message = ex.Message;
            }
            return _response;

        }




        [HttpPost("EmailCartRequest")]
        public async Task<object> EmailCartRequest([FromBody] CartDto cartDto)
        {
            try
            {
                var sendByRabbitMq = _configuration.GetValue<bool>("AllowedPublishers:RabbitMq");
                var sendByServiceBus = _configuration.GetValue<bool>("AllowedPublishers:AzureServiceBus");
                var activeStrategies = new List<IPublishMessage>();

                if (sendByServiceBus)
                    activeStrategies.Add(_messageBusPublisher);

                if (sendByRabbitMq)
                    activeStrategies.Add(_rabbitMqPublisher);

                _messagePublishContext.SetStrategies(activeStrategies.ToArray());
                await _messagePublishContext.PublishManyAsync(_configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue"), cartDto);

                //await _messagePublishContext.PublishAsync(_configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue"), cartDto);

                //await  _messageBus.PublishMessage(_configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue"), cartDto);
              //await  _messageBus.PublishMessage(_configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue"), cartDto);

                _response.Result = true;
                _response.IsSuccuess = true;

            }
            catch (Exception ex)
            {

                _response.IsSuccuess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet("smart-recommendation/{userId}")]
        public async Task<IActionResult> GetSmartRecommendation(string userId= "c17ef2ed-0df8-4f76-b8e3-a15f0ded1d2f")
        {
            var cartItems = await GetItems(userId);
            var prompt = $"Based on these items: {string.Join(", ", cartItems.Select(i => i.Name))}, suggest one complementary product.";

            var recommendation = await _aiService.GetRecommendationAsync(prompt);
            return Ok(recommendation);
        }

        private async Task<List<ProductDto>> GetItems(string userId)
        {
            List<ProductDto> userproducts = new List<ProductDto>();

            try
            {
                CartDto cart = new()
                {
                    CartHeader = _mapper.Map<CartHeaderDto>(_db.CartHeaders.AsNoTracking().FirstOrDefault(a => a.UserId == userId)),

                };
                if (cart.CartHeader != null)
                {
                    cart.CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(_db.CartDetails.AsNoTracking().Where(a => a.CartHeaderId == cart.CartHeader.CartHeaderId));


                    IEnumerable<ProductDto> products = await _productService.GetAllProductsAsync();
                    foreach (var item in cart.CartDetails)
                    {
                        item.Product = products.FirstOrDefault(p => p.ProductId == item.ProductId);

                        
                    }
                    userproducts.AddRange(cart.CartDetails.Select(a=>a.Product));


                }
                else
                {
                   return null;

                }


            }
            catch (Exception ex)
            {

                return null;
            }



            return userproducts;
        }


    }
}
