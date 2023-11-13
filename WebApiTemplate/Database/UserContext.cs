

using Microsoft.EntityFrameworkCore;

namespace WebApiTemplate.Database
{
    public class UserContext : DbContext
    {
       public UserContext(DbContextOptions options) : base(options) { }
       public DbSet<User1>? User1 { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User1>().ToTable("User1");
        }

    }
}
