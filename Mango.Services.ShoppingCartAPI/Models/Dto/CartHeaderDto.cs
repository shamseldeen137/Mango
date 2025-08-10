using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.ShoppingCartAPI.Models.Dto
{
    public class CartHeaderDto
    {
        public int CasrtHeaderId { get; set; }
        public int UserId { get; set; }
        public string CouponCode { get; set; }
        public double Discount { get; set; }
        public double CartTotal { get; set; }
    }
}
