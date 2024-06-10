using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public int IdAlojamiento { get; set; } = 0;

        [Required]
        public int IdInquilino { get; set; } = 0;

        [Required]
        public DateOnly FechaInicio { get; set; }

        [Required]
        public DateOnly FechaFin { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio total debe ser un valor positivo.")]
        public double PrecioTotal { get; set; } = 0;




        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FechaFin < FechaInicio)
            {
                yield return new ValidationResult("La fecha de salida debe ser posterior a la fecha de entrada.", new[] { nameof(FechaFin) });
            }
        }


    }
}
