using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Mango.MessageBus.IMessaging;
using Mango.MessageBus.Utility;
using Newtonsoft.Json;
using RabbitMQ.Client;
namespace Mango.RabbitMQ.Messaging
{
    public class RabbitMqPublisher : IPublishMessage
    {
        private readonly RabbitMQConnectionFactory _factory;

        public RabbitMqPublisher(RabbitMQConnectionFactory factory)
        {
            _factory = factory;
        }

        public async  Task PublishMessage(string queueName, object message)
        {
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
            var jsonMessage = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(jsonMessage);
            channel.BasicPublish(exchange: "", routingKey: queueName, body: body);
        }

    }
}