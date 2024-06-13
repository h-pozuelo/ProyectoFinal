using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Interfaces;
using Server.Models;

namespace Server.Services
{
    public class AlojamientoService : IAlojamientoService
    {
        private readonly ApplicationDbContext _context;

        public AlojamientoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Alojamiento> CreateAlojamiento(Alojamiento alojamiento)
        {
            _context.Alojamientos.Add(alojamiento);
            await _context.SaveChangesAsync();
            return alojamiento;

        }

        public async Task DeleteAlojamiento(int id)
        {
            var alojamientoBorrar = await GetAlojamiento(id);
            if (alojamientoBorrar != null)
            {
                _context.Alojamientos.Remove(alojamientoBorrar);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Alojamiento>> GetAllAlojamientos()
        {
            var alojamientos = await _context.Alojamientos.ToListAsync();
            return alojamientos;

        }

        public async Task<Alojamiento> GetAlojamiento(int id)
        {
            var alojamiento = await _context.Alojamientos
                .FirstOrDefaultAsync(a => a.Id == id);
            return alojamiento;

        }

        public async Task<Alojamiento> UpdateAlojamiento(Alojamiento alojamiento)
        {
            var nuevoAlojamiento = await GetAlojamiento(alojamiento.Id);
            if (nuevoAlojamiento != null)
            {
                nuevoAlojamiento.IdPropietario = alojamiento.IdPropietario;
                nuevoAlojamiento.Direccion = alojamiento.Direccion;//Posible cambio
                nuevoAlojamiento.NumeroHabitaciones = alojamiento.NumeroHabitaciones;
                nuevoAlojamiento.CapacidadInvitados = alojamiento.CapacidadInvitados;
                nuevoAlojamiento.PrecioNoche = alojamiento.PrecioNoche;
                nuevoAlojamiento.Descripcion = alojamiento.Descripcion;

                await _context.SaveChangesAsync();
                return nuevoAlojamiento;
            }
            else
            {
                return null;

            }
        }
    }
}
