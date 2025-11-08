using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infra
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity => {
                entity.HasKey(prod => prod.Id);
                entity.Property(prod => prod.Name)
                    .IsRequired()
                    .HasMaxLength(256);
                entity.Property(prod => prod.Category)
                    .IsRequired()
                    .HasMaxLength(256);
                entity.Property(prod => prod.UnitCost)
                    .IsRequired();
                entity.Property(prod => prod.CreatedAt)
                    .IsRequired();
                entity.Property(prod => prod.IsDeleted)
                    .IsRequired()
                    .HasDefaultValue(false);
            });
        }
    }
}
