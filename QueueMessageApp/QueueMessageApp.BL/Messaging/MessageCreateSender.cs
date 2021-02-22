using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace QueueMessageApp.BL.Messaging
{
    public class MessageCreateSender : IMessageCreateSender
    {
        private readonly IBus _bus;
        private readonly ILogger<IMessageCreateSender> _logger;

        public MessageCreateSender(IBus bus, ILogger<IMessageCreateSender> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        /// <summary>
        /// Sends the message to rabbit mq queue
        /// </summary>
        public async Task SendMessage(PublishMessageModel message, CancellationToken cancellationToken)
        {
            try
            {
                ISendEndpoint endpoint = await _bus.GetPublishSendEndpoint<PublishMessageModel>();
                await endpoint.Send(message, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error sending message to the queue: " + ex.Message);
            }
        }
    }
}
