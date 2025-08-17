using Mango.Services.EmailAPI.Models.Dto;

namespace Mango.Services.EmailAPI.Services
{
    public interface IEmailService
    {
     public Task SendEmailAndLog(CartDto cart,string source);
    }
}
