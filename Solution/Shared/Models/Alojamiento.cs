using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class Alojamiento
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



    }
}
