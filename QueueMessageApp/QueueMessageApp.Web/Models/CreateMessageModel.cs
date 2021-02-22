using System.ComponentModel.DataAnnotations;

namespace QueueMessageApp.Web.Models
{
    public class CreateMessageModel
    {
        [Required]
        public string Content { get; set; }
    }
}
