using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public class AlquilerDto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El alojamiento es obligatorio.")]
        public int IdAlojamiento { get; set; }

        [Required(ErrorMessage = "El inquilino es obligatorio.")]
        public string? IdInquilino { get; set; }

        [Required(ErrorMessage = "La fecha de entrada es obligatoria."),
            DataType(DataType.Date), CompararFechas]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de salida es obligatoria."),
            DataType(DataType.Date), CompararFechas]
        public DateTime FechaFin { get; set; }

        [Required(ErrorMessage = "El precio total es obligatorio."),
            Range(0d, double.MaxValue, ErrorMessage = "El precio total debe ser un valor positivo.")]
        public double PrecioTotal { get; set; }
    }

    public class CompararFechas : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var alquilerDto = (AlquilerDto)validationContext.ObjectInstance;

            if (DateTime.Compare(alquilerDto.FechaFin, alquilerDto.FechaInicio) <= 0)
                return new ValidationResult("La fecha de salida debe ser posterior a la fecha de entrada.");

            return ValidationResult.Success;
        }
    }
}
