using Mango.Services.AuthAPI.Models.Dto;

namespace Mango.Services.AuthAPI.Services.IServices
{
    public interface IAuthService
    {
        Task<string> Register(RegistreRequestDto registreRequestDto);
        Task<LoginResopnseDto> Login(LoginRequestDto loginRequestDto);
        Task<bool> AssignRole(string email ,string roleName);

    
    }
}
