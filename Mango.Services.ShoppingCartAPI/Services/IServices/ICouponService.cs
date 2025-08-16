using Mango.Services.ShoppingCartAPI.Models.Dto;

namespace Mango.Web.Services.IServices
{
    public interface ICouponService
    {
        Task<CouponDto> GetCouponByCodeAsync(string code);


    }
}
