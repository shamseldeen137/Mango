using Mango.Services.EmailAPI.Data;
using Mango.Services.EmailAPI.Models;
using Mango.Services.EmailAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Mango.Services.EmailAPI.Services
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<AppDbContext> _dboptions;

        public EmailService(DbContextOptions<AppDbContext> dboptions)
        {
            this._dboptions = dboptions;
        }

        public async Task SendEmailAndLog(CartDto cartDto,string source)
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine("<br/>Cart Email Requested ");
            message.AppendLine("<br/>Total " + cartDto.CartHeader.CartTotal);
            message.Append("<br/>");
            message.Append("<ul>");
            foreach (var item in cartDto.CartDetails)
            {
                message.Append("<li>");
                message.Append(item.Product.Name + " x " + item.Count);
                message.Append("</li>");
            }
            message.Append("</ul>");

            await LogAndEmail(message.ToString(), cartDto.CartHeader.Email,source);

        }

        private async Task<bool> LogAndEmail(string message, string email, string source)
        {
            try
            {
                EmailLogger emailLogger = new()
                {
                    Email = email,
                    Message = message,
                    EmailSent = DateTime.Now,
                    BrokerSource = source
                };
                await using var _dbContext = new AppDbContext(_dboptions);
                _dbContext.EmailLoggers.Add(emailLogger);
              await  _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
