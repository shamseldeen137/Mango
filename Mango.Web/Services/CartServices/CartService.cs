using Mango.Web.Cart.Models.Dto;
using Mango.Web.Models;
using Mango.Web.Services.BaseServices;
using Mango.Web.Services.IServices;
using Mango.Web.Utility;

namespace Mango.Web.Services.CouponServices
{
    public class CartService :  ICartService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IBaseService _baseService;
        public CartService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto> GetCartByUserId(string userId)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.APIGateWay + "/api/Cart/GetCart/" + userId, // ✅ FIXED
                AccessToken = ""
            });
        }

        public async Task<ResponseDto> UpsertCart(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.APIGateWay + "/api/Cart/CartUpsert", // ✅ FIXED
                AccessToken = ""
            });
        }

        public async Task<ResponseDto>RemoveFromCart(int CartDetilisId)
        {

            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Data = CartDetilisId,
                Url = SD.APIGateWay + "/api/Cart/RemoveFromCart", // ✅ FIXED
                AccessToken = ""
            });

        }

    public  async  Task<ResponseDto> ApplyCoupon(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.APIGateWay + "/api/Cart/ApplyCoupon", // ✅ FIXED
                AccessToken = ""
            });
        }

        public async Task<ResponseDto> EmailCart(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.APIGateWay + "/api/Cart/EmailCartRequest", // ✅ FIXED
                AccessToken = ""
            });
        }

        public async Task<ResponseDto>  DeleteCouponAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.APIGateWay + "/api/Coupon/"+id, // ✅ FIXED
                AccessToken = ""
            });
        }

        public async Task<ResponseDto>  GetAllCouponAsync()
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.APIGateWay+"/api/Coupon/", // ✅ FIXED
                AccessToken = ""
            });
        }

        public async Task<ResponseDto>  GetCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.APIGateWay + $"/api/Coupon/{id}", // ✅ FIXED
                AccessToken = ""
            });
        } 
        public async Task<ResponseDto>  GetCouponByCodeAsync(string code)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.APIGateWay + $"/api/Coupon/GetByCode/{code}", // ✅ FIXED
                AccessToken = ""
            });
        }

        public async Task<ResponseDto>  UpdateCouponAsync(CouponDto CouponDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.PUT,
                Data = CouponDto,
                Url = SD.APIGateWay + "/api/Coupon/", // ✅ FIXED
                AccessToken = ""
            });
        }

           }
}
