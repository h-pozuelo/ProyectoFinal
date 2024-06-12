using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Configuration;
using Server.Models;
using Shared.Models;

namespace Server.Data
{
    // Hereda de "IdentityDbContext" en vez de "DbContext" dado que vamos a implementar autenticación
    public class ApplicationDbContext : IdentityDbContext<Usuario>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new RoleConfiguration());
        }

        public DbSet<Alojamiento> Alojamientos { get; set; }
        public DbSet<Alquiler> Alquileres { get; set; }


    }
}
