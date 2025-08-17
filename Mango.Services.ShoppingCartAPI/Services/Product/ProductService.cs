using Mango.Services.ShoppingCartAPI.Models.Dto;
using Mango.Web.Services.IServices;
using Newtonsoft.Json;

namespace Mango.Services.ShoppingCartAPI.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var client = _httpClientFactory.CreateClient("Product");
            //client.DefaultRequestHeaders.Add(name:"Authorization",)
            var apiResponse = await client.GetAsync("/api/Product");
            
                var content = await apiResponse.Content.ReadAsStringAsync();

                var response = JsonConvert.DeserializeObject<ResponseDto>(content);
                if (response !=null &&response.IsSuccuess)
                {
                    return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(response.Result));
                }
            
            else
            {
                return new List<ProductDto>();
            }
        }

        public Task<ResponseDto> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

    }
}
