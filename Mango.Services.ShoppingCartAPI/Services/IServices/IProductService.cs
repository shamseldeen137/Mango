using Mango.Services.ShoppingCartAPI.Models.Dto;

namespace Mango.Web.Services.IServices
{
    public interface IProductService 
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ResponseDto> GetProductByIdAsync(int id);


    }
}
