using Mango.Services.EmailAPI.Messaging.RabbitMQ;
using Mango.Services.EmailAPI.Models.Dto;
using Mango.Services.EmailAPI.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.EmailAPI.Utility
{
    public class RabbitMqConsumerHostedService : BackgroundService
    {
        private readonly IRabbitMQConsummer _consumer;
        private readonly IConfiguration _configuration;
        private readonly string EmailShoppingCartQueue;
        private readonly EmailService _emailService;

        public RabbitMqConsumerHostedService(IRabbitMQConsummer consumer, IConfiguration configuration, EmailService emailService)
        {
            _consumer = consumer;
            _configuration = configuration;
            EmailShoppingCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");
            _emailService = emailService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.StartConsuming(EmailShoppingCartQueue, OnMessageReceived);

            return Task.CompletedTask;
        }

        private async Task OnMessageReceived(object arg)
        {

            CartDto cart = JsonConvert.DeserializeObject<CartDto>(arg.ToString());
            try
            {
                await _emailService.SendEmailAndLog(cart,SD.RabbitMQ);

                ///TODO



            }
            catch (Exception)
            {
                throw;

            }


        }
    }

}
