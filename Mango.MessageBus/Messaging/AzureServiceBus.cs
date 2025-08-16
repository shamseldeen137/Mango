using Azure.Messaging.ServiceBus;
using Mango.MessageBus.IMessaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mango.MessageBus.Messaging
{
    public class AzureServiceBusPublisher : IPublishMessage
    {
        private readonly string connectionString = "Endpoint=sb://mangoservicebus8.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=GlylhEz7UclOPQ1ybW4/0w1QcJcuMejwX+ASbCtE5GE=";
        public async Task PublishMessage(string queueName, object message)
        {
            await using var client = new ServiceBusClient(connectionString);
            ServiceBusSender sender = client.CreateSender(queueName);
            var jsonMessage = JsonConvert.SerializeObject(message);
            ServiceBusMessage serviceBusMessage = new ServiceBusMessage(
                Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString(),
            }

            ;
            try
            {
                await sender.SendMessageAsync(serviceBusMessage);
                await client.DisposeAsync();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error sending message: {ex.Message}");
            }
        }
    }
}
