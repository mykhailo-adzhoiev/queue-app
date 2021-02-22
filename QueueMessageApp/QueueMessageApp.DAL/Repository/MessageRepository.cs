using Microsoft.EntityFrameworkCore;
using QueueMessageApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QueueMessageApp.DAL.Repository
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(QueueMessageAppDbContext customerContext) : base(customerContext)
        {
        }

        public async Task<List<Message>> GetProcessedMessages(CancellationToken cancellationToken)
        {
            return await _dbContext.Messages.Where(x => x.Processed).ToListAsync(cancellationToken);
        }

        public async Task<Message> GetMessageByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Messages.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}
