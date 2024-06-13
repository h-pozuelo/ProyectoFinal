using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Configuration;
using Server.Models;

namespace Server.Data
{
    // Hereda de "IdentityDbContext" en vez de "DbContext" dado que vamos a implementar autenticación
    public class ApplicationDbContext : IdentityDbContext<Usuario>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RoleConfiguration());

            modelBuilder.Entity<Alojamiento>()
            .HasOne(a => a.Propietario)
            .WithMany(u => u.Alojamientos)
            .HasForeignKey(a => a.IdPropietario)
            .OnDelete(DeleteBehavior.Restrict);

            // Configuración de la relación muchos a muchos entre Usuario y Alojamiento a través de Alquiler
            modelBuilder.Entity<Alquiler>()
                .HasOne(a => a.Alojamiento)
                .WithMany(al => al.Alquileres)
                .HasForeignKey(a => a.IdAlojamiento);

            modelBuilder.Entity<Alquiler>()
                .HasOne(a => a.Inquilino)
                .WithMany(u => u.Alquileres)
                .HasForeignKey(a => a.IdInquilino);
        }

        public DbSet<Alojamiento> Alojamientos { get; set; }
        public DbSet<Alquiler> Alquileres { get; set; }


    }
}
