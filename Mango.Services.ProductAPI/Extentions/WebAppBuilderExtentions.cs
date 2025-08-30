using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Mango.Services.ProductAPI.Extentions
{
    public static class WebAppBuilderExtentions
    {

        public static WebApplicationBuilder AddAppAuthentication(this WebApplicationBuilder builder)
        {

            var Section = builder.Configuration.GetSection("ApiSettings");
            var secret = Section.GetValue<string>("Secret");
            var issuer = Section.GetValue<string>("Issuer");
            var audience = Section.GetValue<string>("Audience");
            // ✅ حماية ضد null في بيئة CI
            if (string.IsNullOrWhiteSpace(secret) || string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
            {
                // تخطي التوثيق في بيئة توليد Swagger
                Console.WriteLine("Skipping JWT setup due to missing configuration (likely running in Swagger CLI context).");
                return builder;
            }


            var key = Encoding.ASCII.GetBytes(secret);
            builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            ValidateAudience = true
        };
    });
            return builder;
        }

    }

}
