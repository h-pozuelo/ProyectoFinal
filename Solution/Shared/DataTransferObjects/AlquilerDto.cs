using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public class AlquilerDto : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Alojamiento")]
        public int IdAlojamiento { get; set; }

        [Required]
        public string IdInquilino { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }

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
