using MediatR;
using QueueMessageApp.DAL.Models;

namespace QueueMessageApp.BL.Command
{
    public class CreateMessageCommand : IRequest<Message>
    {
        public Message Message { get; set; }
    }
}
