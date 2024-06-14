using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public class AlojamientoDto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El Id del propietario es obligatorio.")]
        public string IdPropietario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        public string Direccion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El número de habitaciones es obligatorio.")]
        public int NumeroHabitaciones { get; set; } = 0;

        [Required(ErrorMessage = "La capacidad de invitados es obligatoria.")]
        public int CapacidadInvitados { get; set; } = 0;

        [Required(ErrorMessage = "El precio por noche es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio por noche debe ser un valor positivo.")]
        public double PrecioNoche { get; set; } = 0;

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El título es obligatorio.")]
        public string? Titulo { get; set; }

        [Required(ErrorMessage = "La comunidad es obligatoria.")]
        public string? NombreComunidad { get; set; }

        [Required(ErrorMessage = "La provincia es obligatoria.")]
        public string? NombreProvincia { get; set; }

        [Required(ErrorMessage = "La ciudad es obligatoria.")]
        public string? NombreCiudad { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaPublicacion { get; set; } = DateTime.Now;
    }
}
