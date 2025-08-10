namespace Mango.Services.AuthAPI.Models.Dto
{
    public class LoginResopnseDto
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
    }
}
