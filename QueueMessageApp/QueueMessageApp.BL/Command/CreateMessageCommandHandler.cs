using MediatR;
using QueueMessageApp.BL.Messaging;
using QueueMessageApp.DAL.Models;
using QueueMessageApp.DAL.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace QueueMessageApp.BL.Command
{
    public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, Message>
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMessageCreateSender _messageCreateSender;

        public CreateMessageCommandHandler(IMessageRepository messageRepository, IMessageCreateSender messageCreateSender)
        {
            _messageRepository = messageRepository;
            _messageCreateSender = messageCreateSender;
        }

        /// <summary>
        /// Creates message entity in the database and sends its ID to queue to be handled to consumer
        /// </summary>
        public async Task<Message> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {
            Message message = await _messageRepository.AddAsync(request.Message);
            await _messageCreateSender.SendMessage(new PublishMessageModel { MessageId = message.Id }, cancellationToken);

            return message;
        }
    }
}
