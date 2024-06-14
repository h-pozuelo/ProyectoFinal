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
            var AlquilerUpdate = await GetAlquiler(alquiler.Id);

            if (AlquilerUpdate != null)
            {
                AlquilerUpdate.IdAlojamiento = alquiler.IdAlojamiento;
                AlquilerUpdate.IdInquilino = alquiler.IdInquilino;
                AlquilerUpdate.FechaInicio = alquiler.FechaInicio;
                AlquilerUpdate.FechaFin = alquiler.FechaFin;
                AlquilerUpdate.PrecioTotal = alquiler.PrecioTotal;

                await _context.SaveChangesAsync();
            }
            return AlquilerUpdate;
        }

        public async Task<IEnumerable<Alquiler>> GetAlquileresByAlojamiento(int id)
        {
            var alquilerByAlojamiento = await _context.Alquileres
                .Where(a => a.IdAlojamiento == id)
                .ToListAsync();
            return alquilerByAlojamiento;
        }

        public async Task<IEnumerable<Alquiler>> GetAlquileresByUser(string id)
        {
            var alquilerByUser = await _context.Alquileres
                .Where(a => a.IdInquilino == id)
                .ToListAsync();
            return alquilerByUser;
        }
    }
}
