using Mango.Web.Models;

namespace Mango.Web.Services.IServices
{
    public interface IProductService 
    {
        Task<ResponseDto> GetAllProductsAsync();
        Task<ResponseDto> GetProductByIdAsync(int id);

        Task<ResponseDto> CreateProductAsync(ProductDto CouponDto);
        Task<ResponseDto> UpdateProductAsync(ProductDto CouponDto);
        Task<ResponseDto> DeleteProductAsync(int id);

    }
}
