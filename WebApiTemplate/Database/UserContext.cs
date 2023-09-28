

using Microsoft.EntityFrameworkCore;

namespace WebApiTemplate.Database
{
    public class UserContext : DbContext
    {
       public UserContext(DbContextOptions options) : base(options) { }
       public DbSet<User>? User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
        }

    }
}
