using Microsoft.EntityFrameworkCore;
using EuroLink.Models;

namespace EuroLink.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Phone> Phones { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Phone>()
                .HasIndex(p => new { p.Brand, p.Model })
                .IsUnique();
        }
    }
}