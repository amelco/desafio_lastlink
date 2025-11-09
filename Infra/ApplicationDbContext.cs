using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infra
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductEvent> ProductEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity => {
                entity.HasKey(prod => prod.Id);
                entity.Property(prod => prod.Id)
                    .ValueGeneratedOnAdd();
                entity.Property(prod => prod.Name)
                    .IsRequired()
                    .HasMaxLength(256);
                entity.Property(prod => prod.Category)
                    .IsRequired()
                    .HasMaxLength(256);
                entity.Property(prod => prod.UnitCost)
                    .IsRequired()
                    .HasColumnType("decimal(12,2)");
                entity.Property(prod => prod.CreatedAt)
                    .IsRequired();
                entity.Property(prod => prod.IsDeleted)
                    .IsRequired()
                    .HasDefaultValue(0);

                entity.HasMany(prod => prod.Events)
                    .WithOne(prodEvent => prodEvent.Product)
                    .HasForeignKey(prodEvent => prodEvent.ProductId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<ProductEvent>(entity =>
            {
                entity.HasKey(prod => prod.Id);
                entity.Property(prod => prod.Type)
                    .IsRequired()
                    .HasMaxLength(256);
                entity.Property(prod => prod.ProductId)
                    .IsRequired();
                entity.Property(prod => prod.CreatedAt)
                    .IsRequired();
            });
        }
    }
}
