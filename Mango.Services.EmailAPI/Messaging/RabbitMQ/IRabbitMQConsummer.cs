namespace Mango.Services.EmailAPI.Messaging.RabbitMQ
{
    public interface IRabbitMQConsummer
    {
        void StartConsuming(string queueName, Func<string, Task> onMessageReceived);
    }
}
