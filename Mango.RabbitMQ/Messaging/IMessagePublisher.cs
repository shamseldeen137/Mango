using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mango.RabbitMQ.Messaging
{
    public interface IMessagePublisher
    {
        void Publish(string queueName, object message);
    }

}
