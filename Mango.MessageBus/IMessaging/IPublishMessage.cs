using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mango.MessageBus.IMessaging
{
    public interface IPublishMessage
    {
        Task PublishMessage(string queueName, object message);
    }
}
