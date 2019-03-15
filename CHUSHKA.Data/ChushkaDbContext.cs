using CHUSHKA.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CHUSHKA.Data
{
    public class ChushkaDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public ChushkaDbContext(DbContextOptions<ChushkaDbContext> options)
            : base(options)
        {
        }

        public ChushkaDbContext()
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            base.OnModelCreating(builder);
        }


    }
}
