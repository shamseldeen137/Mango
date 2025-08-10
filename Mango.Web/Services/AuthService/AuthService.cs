using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Mango.Web.Utility;

namespace Mango.Web.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IBaseService _baseService;
        public AuthService(IHttpClientFactory clientFactory,IBaseService baseService) 
        {

            _clientFactory = clientFactory;
            _baseService = baseService;

        }
        public async Task<ResponseDto> AssignRoleAsync(RegisterRequestDto RegisterRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Data = RegisterRequestDto,
                Url = SD.AuthAPIBase + "/api/Auth/AssignRole/", // ✅ FIXED
                AccessToken = ""
            });
        }

        public async Task<ResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Data = loginRequestDto,
                Url = SD.AuthAPIBase + "/api/Auth/Login/", // ✅ FIXED
                AccessToken = ""
            },withBearer:false);
        }

        public async Task<ResponseDto> RegisterAsync(RegisterRequestDto RegisterRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Data = RegisterRequestDto,
                Url = SD.AuthAPIBase + "/api/Auth/register/", // ✅ FIXED
                AccessToken = ""
            }, withBearer: false);
        }
    }
}
