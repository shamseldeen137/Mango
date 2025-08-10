using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.AuthAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
       private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public AuthService(IJwtTokenGenerator jwtTokenGenerator ,AppDbContext db, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == email.ToLower());
            if ( user != null)
            {
                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }

               await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

        public async Task<LoginResopnseDto> Login(LoginRequestDto loginRequestDto)
        {
            if (loginRequestDto == null || string.IsNullOrEmpty(loginRequestDto.UserName) || string.IsNullOrEmpty(loginRequestDto.Password))
            {
                throw new ArgumentException("Invalid login request");
            }
            var user =await _userManager.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());
        
            var isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (isValid==false||user==null)
            {
                return new LoginResopnseDto { Token = "", User = null };
            }
          var roles=await   _userManager.GetRolesAsync(user);
            // Generate JWT token here (not implemented in this example)
            var token = _jwtTokenGenerator.GenerateToken(user,roles); // Placeholder for actual token generation logic
            return new LoginResopnseDto
            {
                User = new UserDto
                {
                    ID = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    PhoneNumber=user.PhoneNumber
                    
                },
                Token = token
            };
        }

        public async Task<string> Register(RegistreRequestDto registreRequestDto)
        {
            if (registreRequestDto == null || string.IsNullOrEmpty(registreRequestDto.Email) || string.IsNullOrEmpty(registreRequestDto.Password))
            {
                throw new ArgumentException("Invalid registration request");
            }
          


                var user = new ApplicationUser
                {
                    UserName = registreRequestDto.Email,
                    Email = registreRequestDto.Email,
                    PhoneNumber = registreRequestDto.PhoneNumber,
                    Name = registreRequestDto.Name
                };
                try
                {
                    var result = await _userManager.CreateAsync(user, registreRequestDto.Password);
                    if (result.Succeeded)
                    {
                        var userToReturn = _db.ApplicationUsers.First(u => u.UserName == registreRequestDto.Email);

                        UserDto userDto = new()
                        {
                            Email = userToReturn.Email,
                            ID = userToReturn.Id,
                            Name = userToReturn.Name,
                            PhoneNumber = userToReturn.PhoneNumber
                        };

                        return "";

                    }
                    else
                    {
                        return result.Errors.FirstOrDefault().Description;
                    }

                }
                catch (Exception ex)
                {

                }
                return "Error Encountered";
            
            }
    }
}
