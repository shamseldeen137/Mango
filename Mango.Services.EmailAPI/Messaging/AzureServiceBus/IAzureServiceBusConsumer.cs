namespace Mango.Services.EmailAPI.Messaging.AzureServiceBus
{
    public interface IAzureServiceBusConsumer
    {
        Task Start();
        Task Stop();
    }
}
