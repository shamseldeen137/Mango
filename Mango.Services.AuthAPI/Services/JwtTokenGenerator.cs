using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Services.IServices;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.AccessControl;
using System.Security.Claims;

namespace Mango.Services.AuthAPI.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions _jwtOptions;
        public JwtTokenGenerator( IOptions<JwtOptions> JwtOptions )
        {
            _jwtOptions=JwtOptions.Value;
        }
        public string GenerateToken(ApplicationUser user,IEnumerable<string> roles)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes(_jwtOptions.Secret);
            var claimlist = new List<Claim>()
            {
                new Claim (JwtRegisteredClaimNames.Name,user.Name),
                new Claim (JwtRegisteredClaimNames.Email,user.Email),
                new Claim (JwtRegisteredClaimNames.Sub,user.Id),
            };
            claimlist.AddRange(roles.Select(role=>new Claim(ClaimTypes.Role,role)));
            var tokendescriptor = new SecurityTokenDescriptor()
            {
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                IssuedAt = DateTime.UtcNow,
                Subject = new ClaimsIdentity(claimlist),
                Expires = DateTime.UtcNow.AddDays(7), // Token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),

            };
            var token = tokenHandler.CreateToken(tokendescriptor);
            // Implementation for generating JWT token goes here
            // This is a placeholder implementation
            return tokenHandler.WriteToken(token);
        }
    
    }
}
