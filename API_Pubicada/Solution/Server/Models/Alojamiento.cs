using Shared.DataTransferObjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class Alojamiento : AlojamientoDto
    {
        [ForeignKey(nameof(IdPropietario))]
        public virtual Usuario? Propietario { get; set; }

        public virtual ICollection<Alquiler>? Alquileres { get; set; }
    }
}
