using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;
using Server.Models;
using Shared.Services;
using AutoMapper;
using Shared.DataTransferObjects;

namespace Server.Services
{
    public class AlquilerService : IAlquilerService
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AlquilerService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<AlquilerDto> CreateAlquiler(AlquilerDto alquiler)
        {
            _context.Alquileres.Add(_mapper.Map<Alquiler>(alquiler));
            await _context.SaveChangesAsync();
            return _mapper.Map<AlquilerDto>(alquiler);
        }

        public async Task DeleteAlquiler(int id)
        {
            //var alquilerBorrar = await GetAlquiler(id);
            var alquilerBorrar = await _context.Alquileres.FindAsync(id);
            if (alquilerBorrar != null)
            {
                _context.Alquileres.Remove(alquilerBorrar);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<AlquilerDto>> GetAllAlquileres()
        {
            var alquileres = _mapper.Map<IEnumerable<AlquilerDto>>(await _context.Alquileres.ToListAsync());
            return alquileres;
        }

        public async Task<AlquilerDto> GetAlquiler(int id)
        {
            var alquiler = _mapper.Map<AlquilerDto>(await _context.Alquileres
                .FirstOrDefaultAsync(a => a.Id == id));
            return alquiler;
        }

        public async Task<AlquilerDto> UpdateAlquiler(AlquilerDto alquiler)
        {
            //var AlquilerUpdate = await GetAlquiler(alquiler.Id);
            var AlquilerUpdate = await _context.Alquileres.FindAsync(alquiler.Id);
            if (AlquilerUpdate != null)
            {
                //AlquilerUpdate.IdAlojamiento = alquiler.IdAlojamiento;
                //AlquilerUpdate.IdInquilino = alquiler.IdInquilino;
                //AlquilerUpdate.FechaInicio = alquiler.FechaInicio;
                //AlquilerUpdate.FechaFin = alquiler.FechaFin;
                //AlquilerUpdate.PrecioTotal = alquiler.PrecioTotal;

                _mapper.Map<AlquilerDto, Alquiler>(alquiler, AlquilerUpdate);

                await _context.SaveChangesAsync();
                return _mapper.Map<AlquilerDto>(AlquilerUpdate);
            }
            else
            {
                return null;
            }
            //return AlquilerUpdate;
        }

        public async Task<IEnumerable<AlquilerDto>> GetAlquileresByAlojamiento(int id)
        {
            var alquilerByAlojamiento = _mapper.Map<IEnumerable<Alquiler>>(await _context.Alquileres
                .Where(a => a.IdAlojamiento == id)
                .ToListAsync());
            return alquilerByAlojamiento;
        }

        public async Task<IEnumerable<AlquilerDto>> GetAlquileresByUser(string id)
        {
            var alquilerByUser = _mapper.Map<IEnumerable<AlquilerDto>>(await _context.Alquileres
                .Where(a => a.IdInquilino == id)
                .ToListAsync());
            return alquilerByUser;
        }
    }
}
