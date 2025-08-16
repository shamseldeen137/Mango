using Mango.MessageBus.Models;
using Mango.RabbitMQ.Messaging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Mango.MessageBus.IMessaging;


namespace Mango.MessageBus.Utility
{
    public static class RabbitMQServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbitMQMessaging(this IServiceCollection services, IConfiguration config)
        {
            var settings = config.GetSection("RabbitMQ").Get<RabbitMQSettings>();
            services.AddSingleton(settings);
            services.AddSingleton<RabbitMQConnectionFactory>();
            services.AddSingleton<IPublishMessage, RabbitMqPublisher>();
            return services;
        }
    }
}
