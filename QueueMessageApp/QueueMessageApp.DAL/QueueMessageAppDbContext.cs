using Microsoft.EntityFrameworkCore;
using QueueMessageApp.DAL.Models;

namespace QueueMessageApp.DAL
{
    public sealed class QueueMessageAppDbContext : DbContext
    {
        public DbSet<Message> Messages { get; set; }

        public QueueMessageAppDbContext(DbContextOptions<QueueMessageAppDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
