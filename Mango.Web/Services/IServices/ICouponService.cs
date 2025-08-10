using Mango.Web.Models;

namespace Mango.Web.Services.IServices
{
    public interface ICouponService 
    {
        Task<ResponseDto> GetAllCouponAsync();
        Task<ResponseDto> GetCouponByIdAsync(int id);
        Task<ResponseDto> GetCouponByCodeAsync(string code);

        Task<ResponseDto> CreateCouponAsync(CouponDto CouponDto);
        Task<ResponseDto> UpdateCouponAsync(CouponDto CouponDto);
        Task<ResponseDto> DeleteCouponAsync(int id);

    }
}
