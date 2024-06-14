using Shared.DataTransferObjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class Alojamiento : AlojamientoDto
    {

        public virtual IEnumerable<Alquiler>? Alquileres { get; set; }
        [ForeignKey(nameof(IdPropietario))]
        public virtual Usuario? Propietario { get; set; }


    }
}
