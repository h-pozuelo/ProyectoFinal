using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Configuration;
using Server.Models;

namespace Server.Data
{
    // Hereda de "IdentityDbContext" en vez de "DbContext" dado que vamos a implementar autenticación
    public class ApplicationDbContext : IdentityDbContext<Usuario>
    {
        public DbSet<Alojamiento> Alojamientos { get; set; }
        public DbSet<Alquiler> Alquileres { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new RoleConfiguration());

            builder.Entity<Alojamiento>()
                .HasOne(a => a.Propietario)
                .WithMany(p => p.Alojamientos)
                .HasForeignKey(a => a.IdPropietario)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Alquiler>()
                .HasOne(al => al.Alojamiento)
                .WithMany(a => a.Alquileres)
                .HasForeignKey(al => al.IdAlojamiento);

            builder.Entity<Alquiler>()
                .HasOne(al => al.Inquilino)
                .WithMany(i => i.Alquileres)
                .HasForeignKey(al => al.IdInquilino);
        }
    }
}
