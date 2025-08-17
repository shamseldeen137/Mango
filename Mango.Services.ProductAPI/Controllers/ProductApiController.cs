using AutoMapper;
using Mango.Services.ProductAPI.Models.Dto;
using Mango.Services.ProductAPI.Repos.IRepos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/Product")]
    [ApiController]
   // [Authorize]
    public class ProductApiController : ControllerBase
    {
        private readonly IProductRepo _productRepo;
        private readonly IMapper _mapper;
        private readonly ResponseDto _responseDto;

        public ProductApiController(IProductRepo productRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }

        [HttpGet]
        public async Task<ResponseDto> GetProducts()
        {
            try
            {
                var products = await _productRepo.GetProducts();
                var result = _mapper.Map<List<ProductDto>>(products);
                _responseDto.Result = result;
                _responseDto.IsSuccuess = true;
                _responseDto.Message = "Success";
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccuess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpGet("{productId:int}")]
        public async Task<ResponseDto> GetProductById(int productId)
        {
            try
            {
                var product = await _productRepo.GetProductById(productId);
                if (product == null)
                {
                    _responseDto.IsSuccuess = false;
                    _responseDto.Message = "Product not found";
                    return _responseDto;
                }

                var result = _mapper.Map<ProductDto>(product);
                _responseDto.Result = result;
                _responseDto.IsSuccuess = true;
                _responseDto.Message = "Success";
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccuess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ResponseDto> CreateProduct([FromBody] ProductDto productDto)
        {
            try
            {
                var product = await _productRepo.CreateUpdateProduct(productDto);
                var result = _mapper.Map<ProductDto>(product);
                _responseDto.Result = result;
                _responseDto.IsSuccuess = true;
                _responseDto.Message = "Product created successfully";
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccuess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public async Task<ResponseDto> UpdateProduct([FromBody] ProductDto productDto)
        {
            try
            {
                var product = await _productRepo.CreateUpdateProduct(productDto);
                var result = _mapper.Map<ProductDto>(product);
                _responseDto.Result = result;
                _responseDto.IsSuccuess = true;
                _responseDto.Message = "Product updated successfully";
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccuess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpDelete("{productId:int}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ResponseDto> DeleteProduct(int productId)
        {
            try
            {
                var isSuccess = await _productRepo.DeleteProduct(productId);
                if (!isSuccess)
                {
                    _responseDto.IsSuccuess = false;
                    _responseDto.Message = "Product not found or could not be deleted";
                    return _responseDto;
                }

                _responseDto.Result = productId;
                _responseDto.IsSuccuess = true;
                _responseDto.Message = "Product deleted successfully";
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccuess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }
    }
}