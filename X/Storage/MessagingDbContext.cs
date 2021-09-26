using Microsoft.EntityFrameworkCore;

namespace X.Storage
{
    public class MessagingDbContext : DbContext
    {
        public MessagingDbContext(DbContextOptions<MessagingDbContext> options)
            : base(options) { }

        public DbSet<Models.Chat> Chats { get; set; }
        public DbSet<Models.User> Users { get; set; }
        public DbSet<Models.Message> Messages { get; set; }
    }
}
