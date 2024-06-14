using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Shared.DataTransferObjects;

namespace Server.Models
{
    public class Alquiler : AlquilerDto
    {

        public virtual Alojamiento? Alojamiento { get; set; }
        [ForeignKey(nameof(IdInquilino))]
        public virtual Usuario? Inquilino { get; set; }


    }
}
