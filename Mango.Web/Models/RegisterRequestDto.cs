using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models
{
    public class RegisterRequestDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }

    }
}
