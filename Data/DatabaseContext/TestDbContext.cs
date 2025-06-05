using atf.Data.Models;
using Microsoft.EntityFrameworkCore;


namespace atf.Data.DatabaseContext
{
    public class TestDbContext : DbContext
    {
        /// <summary>
        /// Entity Framework Core database context for the application's data models.
        /// Configures the <see cref="User"/> and <see cref="Product"/> entities.
        /// </summary>
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            });
        }
    }
}
