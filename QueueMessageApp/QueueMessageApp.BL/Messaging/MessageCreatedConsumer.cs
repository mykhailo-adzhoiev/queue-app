using MassTransit;
using QueueMessageApp.DAL.Models;
using QueueMessageApp.DAL.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QueueMessageApp.BL.Messaging
{
    public class MessageCreatedConsumer : IConsumer<PublishMessageModel>
    {
        // Can be moved to configuration or to a database, based on system requirements
        private readonly string[] _stopWords = { "dog", "cat" };
        private const string SymbolToReplace = "*";

        private readonly IMessageRepository _messageRepository;

        public MessageCreatedConsumer(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        /// <summary>
        /// Reads the data from rabbit mq queue (messageId), search by message id in database and replaces stop words with *
        /// </summary>
        public async Task Consume(ConsumeContext<PublishMessageModel> context)
        {
            Guid messageId = context.Message.MessageId;

            Message message = await _messageRepository.GetMessageByIdAsync(messageId, context.CancellationToken);

            string messageContent = message.Content.ToLower();

            if (_stopWords.Any(x => messageContent.Contains(x.ToLower())))
            {
                messageContent = _stopWords.Aggregate(messageContent, (current, word) => current.Replace(word, SymbolToReplace));

                message.Content = messageContent;
                message.Processed = true;

                Console.WriteLine($"Processed message with id {messageId}");
                await _messageRepository.UpdateAsync(message);
            }
        }
    }
}
