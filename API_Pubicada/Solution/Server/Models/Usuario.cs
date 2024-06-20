using Microsoft.AspNetCore.Identity;

namespace Server.Models
{
    // Hereda de la clase "IdentityUser" por que queremos agregar propiedades adicionales al momento de construir la tabla "AspNetUsers"
    public class Usuario : IdentityUser
    {
        public string? NombreCompleto { get; set; }
        public DateTime FechaRegistro { get; set; }

        public virtual ICollection<Alojamiento>? Alojamientos { get; set; }

        public virtual ICollection<Alquiler>? Alquileres { get; set; }
    }
}
