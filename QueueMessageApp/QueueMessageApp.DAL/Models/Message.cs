using System;

namespace QueueMessageApp.DAL.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        /// <summary>
        /// Indicates if stop words were replace in message Content (via rabbit mq consumer)
        /// </summary>
        public bool Processed { get; set; }
    }
}
