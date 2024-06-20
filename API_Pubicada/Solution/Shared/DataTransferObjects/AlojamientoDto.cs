using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public class AlojamientoDto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio.")]
        public string? Titulo { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        public string? Direccion { get; set; }

        [Required(ErrorMessage = "La comunidad es obligatoria.")]
        public string? NombreComunidad { get; set; }

        [Required(ErrorMessage = "La provincia es obligatoria.")]
        public string? NombreProvincia { get; set; }

        [Required(ErrorMessage = "La ciudad es obligatoria.")]
        public string? NombreCiudad { get; set; }

        [Required(ErrorMessage = "El número de habitaciones es obligatorio."),
            Range(0, int.MaxValue, ErrorMessage = "El número de habitaciones debe ser un valor positivo.")]
        public int NumeroHabitaciones { get; set; }

        [Required(ErrorMessage = "La capacidad de invitados es obligatoria."),
            Range(0, int.MaxValue, ErrorMessage = "La capacidad de invitados debe ser un valor positivo.")]
        public int CapacidadInvitados { get; set; }

        [Required(ErrorMessage = "El precio por noche es obligatorio."),
            Range(0d, double.MaxValue, ErrorMessage = "El precio por noche debe ser un valor positivo.")]
        public double PrecioNoche { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaPublicacion { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El propietario es obligatorio.")]
        public string? IdPropietario { get; set; }
    }
}
