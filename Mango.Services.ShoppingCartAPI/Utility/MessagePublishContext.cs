using Mango.MessageBus.IMessaging;
using Mango.RabbitMQ.Messaging;

namespace Mango.Services.ShoppingCartAPI.Utility
{
    public class MessagePublishContext
    {
        private IPublishMessage _strategy;

        public void SetStrategy(IPublishMessage strategy)
        {
            _strategy = strategy;
        }

        public async Task PublishAsync(string QueueName, object message)
        {
            if (_strategy == null)
                throw new InvalidOperationException("Publishing strategy not set.");

             await _strategy.PublishMessage( QueueName,message);
        }


        private readonly List<IPublishMessage> _strategies = new();

        public void SetStrategies(params IPublishMessage[] strategies)
        {
            _strategies.Clear();
            _strategies.AddRange(strategies);
        }

        public async Task PublishManyAsync(string QueueName, object message)
        {
            foreach (var strategy in _strategies)
            {
                await strategy.PublishMessage( QueueName,message);
            }
        }



    }
}
