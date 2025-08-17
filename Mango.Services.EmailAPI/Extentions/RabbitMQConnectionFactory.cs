using Mango.Services.EmailAPI.Models.Dto.RabbitMQ;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Services.EmailAPI.Extentions
{
    public class RabbitMQConnectionFactory
    {
        private readonly RabbitMQSettings _settings;

        public RabbitMQConnectionFactory(RabbitMQSettings settings)
        {
            _settings = settings;
        }

        public IConnection CreateConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.Host,
                Port = _settings.Port,
                UserName = _settings.Username,
                Password = _settings.Password,
                VirtualHost = _settings.VirtualHost
            };

            return factory.CreateConnection();
        }
    }

}
