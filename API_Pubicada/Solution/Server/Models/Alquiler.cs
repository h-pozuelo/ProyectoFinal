using Shared.DataTransferObjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class Alquiler : AlquilerDto
    {
        [ForeignKey(nameof(IdAlojamiento))]
        public virtual Alojamiento? Alojamiento { get; set; }

        [ForeignKey(nameof(IdInquilino))]
        public virtual Usuario? Inquilino { get; set; }
    }
}
