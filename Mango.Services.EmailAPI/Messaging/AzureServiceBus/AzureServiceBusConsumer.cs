using Azure.Messaging.ServiceBus;
using Mango.Services.EmailAPI.Models.Dto;
using Mango.Services.EmailAPI.Models.Dto.RabbitMQ;
using Mango.Services.EmailAPI.Services;
using Mango.Services.EmailAPI.Utility;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.EmailAPI.Messaging.AzureServiceBus
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string AzureConnectionString;
        private readonly string EmailShoppingCartQueue;
        private readonly IConfiguration _configuration;
        private readonly RabbitMQSettings _rabbitMQSettings;
        private readonly ServiceBusProcessor _serviceBusProcessor;
        private readonly EmailService _emailService;
        public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration;

            AzureConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            EmailShoppingCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");
            _rabbitMQSettings = _configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>();
            var client = new ServiceBusClient(AzureConnectionString);
            _serviceBusProcessor = client.CreateProcessor(EmailShoppingCartQueue);
            _emailService = emailService;
        }

        public async Task Start()
        {
            try
            {
                _serviceBusProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
                _serviceBusProcessor.ProcessErrorAsync += ErrorHandler;
                await _serviceBusProcessor.StartProcessingAsync();
            }
            catch (Exception e)
            {

                throw;
            }

        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs arg)
        {
            var messsage = arg.Message;
            var body = Encoding.UTF8.GetString(messsage.Body);
            CartDto cart = JsonConvert.DeserializeObject<CartDto>(body);
            try
            {
                ///TODO
              await  _emailService.SendEmailAndLog(cart,SD.AzureServiceBus);

                await arg.CompleteMessageAsync(arg.Message);

            }
            catch (Exception)
            {
                throw;

            }

        }

        public async Task Stop()
        {
            await _serviceBusProcessor.StopProcessingAsync();
            await _serviceBusProcessor.DisposeAsync();
        }
    }
}
