using Mango.Web.Cart.Models.Dto;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            this._cartService = cartService;
        }
        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartDataForLoggedUser());
        }
        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var response = await _cartService.RemoveFromCart(cartDetailsId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Item removed from cart successfully.";
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return RedirectToAction(nameof(CartIndex));
        }
        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            var response = await _cartService.ApplyCoupon(cartDto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon Added successfully.";
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return RedirectToAction(nameof(CartIndex));
} 
       [HttpPost]
        public async Task<IActionResult> EmailCart(CartDto cartDto)
        {
            CartDto cart = await LoadCartDataForLoggedUser();
            cart.CartHeader.Email = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email)?.Value;


            var response = await _cartService.EmailCart(cart);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Email Will be sent shortley.";
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return RedirectToAction(nameof(CartIndex));
} 
        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            cartDto.CartHeader.CouponCode = "";
            var response = await _cartService.ApplyCoupon(cartDto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon Added successfully.";
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return RedirectToAction(nameof(CartIndex));
}
        public async Task<CartDto> LoadCartDataForLoggedUser()
        {
            string userid = GetUserId();
            var response = await _cartService.GetCartByUserId(userid);

            if (response != null && response.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
                return new CartDto();
            }

        }
        private string GetUserId()
        {

            var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (userId == null)
            {
                throw new UnauthorizedAccessException("User is not authenticated");
            }
            return userId;

        }
    }
}
