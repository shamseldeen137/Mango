using Mango.Services.ShoppingCartAPI.Models.Dto;
using Mango.Web.Services.IServices;
using Newtonsoft.Json;

namespace Mango.Services.ShoppingCartAPI.Services.Product
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<CouponDto> GetCouponByCodeAsync(string Code)
        {
            var client = _httpClientFactory.CreateClient("Coupon");
            //client.DefaultRequestHeaders.Add(name:"Authorization",)
            var apiResponse = await client.GetAsync($"/api/CouponApi/GetByCode/{Code}");
            
                var content = await apiResponse.Content.ReadAsStringAsync();

                var response = JsonConvert.DeserializeObject<ResponseDto>(content);
                if (response !=null &&response.IsSuccuess)
                {
                    return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));
                }
            
            else
            {
                return new CouponDto();
            }
        }

    }
}
