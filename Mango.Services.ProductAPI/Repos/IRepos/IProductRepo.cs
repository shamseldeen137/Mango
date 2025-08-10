using Mango.Services.ProductAPI.Models.Dto;

namespace Mango.Services.ProductAPI.Repos.IRepos
{
    public interface IProductRepo
    {
        Task<IEnumerable<ProductDto>> GetProducts();
        Task<ProductDto> GetProductById(int ProductId);
        Task<ProductDto> CreateUpdateProduct(ProductDto productDto);
        Task<bool> DeleteProduct(int ProductId);
    }
}
