using System.Threading;
using System.Threading.Tasks;

namespace QueueMessageApp.BL.Messaging
{
    public interface IMessageCreateSender
    {
        /// <summary>
        /// Sends message to Rabbit MQ queue using MassTransit Bus
        /// </summary>
        Task SendMessage(PublishMessageModel message, CancellationToken cancellationToken);
    }
}
