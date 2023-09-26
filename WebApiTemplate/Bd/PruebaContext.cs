

using Microsoft.EntityFrameworkCore;

namespace WebApiTemplate.Bd
{
    public class PruebaContext : DbContext
    {
       public PruebaContext(DbContextOptions options) : base(options) { }
       public DbSet<Usuario>? Usuario { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().ToTable("Usuario");
        }

    }
}
