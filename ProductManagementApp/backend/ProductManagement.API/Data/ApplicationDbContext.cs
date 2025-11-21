using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductManagement.API.Models;

namespace ProductManagement.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Descripcion).HasMaxLength(500);
                entity.Property(p => p.Precio).HasColumnType("decimal(18,2)");
                entity.Property(p => p.Estado).IsRequired();
                entity.Property(p => p.UsuarioCreacion).IsRequired();
                entity.Property(p => p.FechaCreacion).IsRequired();
            });
        }
    }
}