using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;
using Server.Models;
using Server.Interfaces;

namespace Server.Services
{
    public class AlquilerService : IAlquilerService
    {

        private readonly ApplicationDbContext _context;

        public AlquilerService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Alquiler> CreateAlquiler(Alquiler alquiler)
        {
            _context.Alquileres.Add(alquiler);
            await _context.SaveChangesAsync();
            return alquiler;
        }

        public async Task DeleteAlquiler(int id)
        {
            var alquilerBorrar = await GetAlquiler(id);
            if (alquilerBorrar != null)
            {
                _context.Alquileres.Remove(alquilerBorrar);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Alquiler>> GetAllAlquileres()
        {
            var alquileres = await _context.Alquileres.ToListAsync();
            return alquileres;
        }

        public async Task<Alquiler> GetAlquiler(int id)
        {
            var alquiler = await _context.Alquileres
                .FirstOrDefaultAsync(a => a.Id == id);
            return alquiler;
        }

        public async Task<Alquiler> UpdateAlquiler(Alquiler alquiler)
        {
            var nuevoAlquiler = await GetAlquiler(alquiler.Id);

            if (nuevoAlquiler != null)
            {
                nuevoAlquiler.IdAlojamiento = alquiler.IdAlojamiento;
                nuevoAlquiler.IdInquilino = alquiler.IdInquilino;
                nuevoAlquiler.FechaInicio = alquiler.FechaInicio;
                nuevoAlquiler.FechaFin = alquiler.FechaFin;
                nuevoAlquiler.PrecioTotal = alquiler.PrecioTotal;

                await _context.SaveChangesAsync();
            }
            return nuevoAlquiler;
        }
    }
}
