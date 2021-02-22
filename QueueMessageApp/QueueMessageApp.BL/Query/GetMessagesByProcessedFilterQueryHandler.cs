using MediatR;
using QueueMessageApp.DAL.Models;
using QueueMessageApp.DAL.Repository;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace QueueMessageApp.BL.Query
{
    public class GetMessagesByProcessedFilterQueryHandler : IRequestHandler<GetMessagesByProcessedFilterQuery, List<Message>>
    {
        private readonly IMessageRepository _messageRepository;

        public GetMessagesByProcessedFilterQueryHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        /// <summary>
        /// Gets the list of processed via rabbit mq consumer messages
        /// </summary>
        public async Task<List<Message>> Handle(GetMessagesByProcessedFilterQuery request, CancellationToken cancellationToken)
        {
            return await _messageRepository.GetProcessedMessages(cancellationToken);
        }
    }
}
