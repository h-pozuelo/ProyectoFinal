using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class Alquiler : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Alojamiento")]
        public int IdAlojamiento { get; set; }

        [Required]
        //[ForeignKey("Inquilino")]
        public string IdInquilino { get; set; }

        [Required]
        public DateOnly FechaInicio { get; set; }

        [Required]
        public DateOnly FechaFin { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio total debe ser un valor positivo.")]
        public double PrecioTotal { get; set; } = 0;

        public virtual Alojamiento? Alojamiento { get; set; }
        //public virtual Usuario? Inquilino { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FechaFin < FechaInicio)
            {
                yield return new ValidationResult("La fecha de salida debe ser posterior a la fecha de entrada.", new[] { nameof(FechaFin) });
            }
        }


    }
}
