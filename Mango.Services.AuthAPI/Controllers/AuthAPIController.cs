using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthAPIController:ControllerBase
        
    {
       private readonly IAuthService _authService;
       protected  ResponseDto _responseDto;
        public AuthAPIController(IAuthService authService )
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
_responseDto = new ResponseDto();
        }
        [HttpPost("register")]
        public async Task< IActionResult> Register([FromBody] RegistreRequestDto registrationData)
        {
          var registerresponse=await  _authService.Register(registrationData);
            if (!string.IsNullOrEmpty( registerresponse)) { 
                _responseDto.IsSuccess = false;
                _responseDto.Result=registerresponse;
                return BadRequest(_responseDto);
            }
            else { 
            _responseDto.IsSuccess = true;
            _responseDto.Message = "User registered successfully";
            // Logic for user registration
            return Ok(_responseDto);
            }
        }
        [HttpPost("login")]
        public async Task< IActionResult> login([FromBody] LoginRequestDto loginDto)
        {
            LoginResopnseDto loginresponse = await _authService.Login(loginDto);
            if (loginresponse.User==null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Username Or Password incorrect";
                return BadRequest(_responseDto);
            }
            _responseDto.IsSuccess = true;
            _responseDto.Result = loginresponse;
            // Logic for user registration
            return Ok(_responseDto);
        }
        [HttpPost("AssignRole")]
        public async Task< IActionResult> AssignRole(RegistreRequestDto model)
        {
            var IsAssigned = await _authService.AssignRole(model.Email,model.Role.ToUpper());
            if (!IsAssigned)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Error Encountered";
                return BadRequest(_responseDto);
            }
            _responseDto.IsSuccess = true;
            _responseDto.Result = IsAssigned;
            // Logic for user registration
            return Ok(_responseDto);
        }

    }
}
