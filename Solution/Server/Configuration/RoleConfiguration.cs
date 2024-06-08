using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Server.Configuration
{
    // Hereda de la clase "IEntityTypeConfiguration" del tipo "IdentityRole"
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        // Método para agregar registros iniciales al momento de construir la tabla "AspNetRoles"
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Name = "Visitor",
                    NormalizedName = "VISITOR"
                },
                new IdentityRole
                {
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                });
        }
    }
}
