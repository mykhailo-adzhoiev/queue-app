using MediatR;
using QueueMessageApp.DAL.Models;
using System.Collections.Generic;

namespace QueueMessageApp.BL.Query
{
    public class GetMessagesByProcessedFilterQuery : IRequest<List<Message>>
    {
        public GetMessagesByProcessedFilterQuery(bool processed)
        {
            Processed = processed;
        }

        public bool Processed { get; set; }
    }
}
