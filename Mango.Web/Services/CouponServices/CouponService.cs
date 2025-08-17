using Mango.Web.Models;
using Mango.Web.Services.BaseServices;
using Mango.Web.Services.IServices;
using Mango.Web.Utility;

namespace Mango.Web.Services.CouponServices
{
    public class CouponService :  ICouponService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IBaseService _baseService;
        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }


        public async Task<ResponseDto>  CreateCouponAsync(CouponDto CouponDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Data = CouponDto,
                Url = SD.CouponAPIBase + "/api/Coupon/", // ✅ FIXED
                AccessToken = ""
            });
        }

        public async Task<ResponseDto>  DeleteCouponAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.CouponAPIBase + "/api/Coupon/"+id, // ✅ FIXED
                AccessToken = ""
            });
        }

        public async Task<ResponseDto>  GetAllCouponAsync()
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponAPIBase+"/api/Coupon/", // ✅ FIXED
                AccessToken = ""
            });
        }

        public async Task<ResponseDto>  GetCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponAPIBase + $"/api/Coupon/{id}", // ✅ FIXED
                AccessToken = ""
            });
        } 
        public async Task<ResponseDto>  GetCouponByCodeAsync(string code)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponAPIBase + $"/api/Coupon/GetByCode/{code}", // ✅ FIXED
                AccessToken = ""
            });
        }

        public async Task<ResponseDto>  UpdateCouponAsync(CouponDto CouponDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.PUT,
                Data = CouponDto,
                Url = SD.CouponAPIBase + "/api/Coupon/", // ✅ FIXED
                AccessToken = ""
            });
        }

    }
}
