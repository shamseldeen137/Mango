using Mango.Services.EmailAPI.Models.Dto.RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;

namespace Mango.Services.EmailAPI.Extentions
{
    public class RabbitMQConnectionFactory
    {
        private readonly RabbitMQSettings _settings;
        private readonly ILogger<RabbitMQConnectionFactory> _logger;

        public RabbitMQConnectionFactory(RabbitMQSettings settings, ILogger<RabbitMQConnectionFactory> logger)
        {
            _settings = settings;
            _logger = logger;
        }

        public IConnection CreateConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.Host,
                Port = _settings.Port,
                UserName = _settings.Username,
                Password = _settings.Password,
                VirtualHost = _settings.VirtualHost,
                RequestedConnectionTimeout = TimeSpan.FromSeconds(10)
            };

            const int maxRetries = 5;
            const int delaySeconds = 3;

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    _logger.LogInformation("Attempting RabbitMQ connection (Attempt {Attempt}/{Max})...", attempt, maxRetries);
                    return factory.CreateConnection();
                }
                catch (BrokerUnreachableException ex) when (ex.InnerException is SocketException)
                {
                    _logger.LogWarning("RabbitMQ unreachable: {Message}", ex.Message);
                    if (attempt == maxRetries)
                        throw;

                    Thread.Sleep(TimeSpan.FromSeconds(delaySeconds));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error while connecting to RabbitMQ.");
                    throw;
                }
            }

            throw new Exception("Failed to connect to RabbitMQ after retries.");
        }
    }
}