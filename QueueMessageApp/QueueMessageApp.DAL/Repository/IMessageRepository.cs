using QueueMessageApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace QueueMessageApp.DAL.Repository
{
    public interface IMessageRepository : IRepository<Message>
    {
        /// <summary>
        /// Gets the list of processed messages (Processed = true)
        /// </summary>
        Task<List<Message>> GetProcessedMessages(CancellationToken cancellationToken);
        /// <summary>
        /// Gets the message by ID
        /// </summary>
        Task<Message> GetMessageByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
