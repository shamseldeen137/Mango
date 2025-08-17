
using Mango.Services.EmailAPI.Extentions;
using Mango.Services.EmailAPI.Models.Dto.RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;


namespace Mango.Services.EmailAPI.Messaging.RabbitMQ
{
    public class RabbitMQConsumer:IRabbitMQConsummer
    {
        private readonly RabbitMQConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQConsumer(RabbitMQConnectionFactory factory)
        {
            _factory = factory;
        }


        public void StartConsuming(string queueName, Func<string, Task> onMessageReceived)
        {
            // Implementation for consuming messages from RabbitMQ queue
            // This method should connect to RabbitMQ, subscribe to the specified queue,
            // and invoke the onMessageReceived callback when a message is received.

            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                await onMessageReceived(message);
            };

            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);


        }
    }
}
