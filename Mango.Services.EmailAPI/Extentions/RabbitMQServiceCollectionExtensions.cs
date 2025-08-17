
using Mango.Services.EmailAPI.Models.Dto.RabbitMQ;
using Mango.Services.EmailAPI.Messaging.RabbitMQ;
using Mango.Services.EmailAPI.Utility;

namespace Mango.Services.EmailAPI.Extentions
{
    public static class RabbitMQServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbitMQMessaging(this IServiceCollection services, IConfiguration config)
        {
            var settings = config.GetSection("RabbitMQ").Get<RabbitMQSettings>();
            services.AddSingleton(settings);
            services.AddSingleton<RabbitMQConnectionFactory>();
            services.AddSingleton<IRabbitMQConsummer, RabbitMQConsumer>();
            services.AddHostedService<RabbitMqConsumerHostedService>();

            return services;
        }
    }
}
