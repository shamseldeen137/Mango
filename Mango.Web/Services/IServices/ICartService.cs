using Mango.Web.Cart.Models.Dto;
using Mango.Web.Models;

namespace Mango.Web.Services.IServices
{
    public interface ICartService
    {
        Task<ResponseDto> GetCartByUserId(string userId);
        Task<ResponseDto> UpsertCart(CartDto cartDto);
        Task<ResponseDto> RemoveFromCart(int CartDetilisId);
        Task<ResponseDto> ApplyCoupon(CartDto cartDto);
        Task<ResponseDto> EmailCart(CartDto cartDto);

      
    }
}
