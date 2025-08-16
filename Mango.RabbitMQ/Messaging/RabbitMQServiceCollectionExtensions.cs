using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mango.RabbitMQ.Messaging
{
    public static class RabbitMQServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbitMQMessaging(this IServiceCollection services, IConfiguration config)
        {
            var settings = config.GetSection("RabbitMQ").Get<RabbitMQSettings>();
            services.AddSingleton(settings);
            services.AddSingleton<RabbitMQConnectionFactory>();
            services.AddSingleton<IMessagePublisher, MessagePublisher>();
            return services;
        }
    }
}
