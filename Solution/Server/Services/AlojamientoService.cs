using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Shared.Services;
using Server.Models;
using Shared.DataTransferObjects;

namespace Server.Services
{
    public class AlojamientoService : IAlojamientoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AlojamientoService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AlojamientoDto> CreateAlojamiento(AlojamientoDto alojamiento)
        {
            var result = _context.Alojamientos.Add(_mapper.Map<Alojamiento>(alojamiento));
            await _context.SaveChangesAsync();
            return _mapper.Map<AlojamientoDto>(result);

        }

        public async Task DeleteAlojamiento(int id)
        {
            //var alojamientoBorrar = await GetAlojamiento(id);
            var alojamientoBorrar = await _context.Alojamientos.FindAsync(id);
            if (alojamientoBorrar != null)
            {
                _context.Alojamientos.Remove(alojamientoBorrar);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<AlojamientoDto>> GetAllAlojamientos()
        {
            var alojamientos = _mapper.Map<IEnumerable<AlojamientoDto>>(await _context.Alojamientos.ToListAsync());
            return alojamientos;

        }

        public async Task<AlojamientoDto> GetAlojamiento(int id)
        {
            var alojamiento = _mapper.Map<AlojamientoDto>(await _context.Alojamientos
                .FirstOrDefaultAsync(a => a.Id == id));
            return alojamiento;

        }


        public async Task<AlojamientoDto> UpdateAlojamiento(AlojamientoDto alojamiento)
        {
            //var AlojamientoUpdate = await GetAlojamiento(alojamiento.Id);
            var AlojamientoUpdate = await _context.Alojamientos.FindAsync(alojamiento.Id);
            if (AlojamientoUpdate != null)
            {
                //AlojamientoUpdate.IdPropietario = alojamiento.IdPropietario;
                //AlojamientoUpdate.Direccion = alojamiento.Direccion;//Posible cambio
                //AlojamientoUpdate.NumeroHabitaciones = alojamiento.NumeroHabitaciones;
                //AlojamientoUpdate.CapacidadInvitados = alojamiento.CapacidadInvitados;
                //AlojamientoUpdate.PrecioNoche = alojamiento.PrecioNoche;
                //AlojamientoUpdate.Descripcion = alojamiento.Descripcion;

                _mapper.Map<AlojamientoDto, Alojamiento>(alojamiento, AlojamientoUpdate);

                await _context.SaveChangesAsync();
                return _mapper.Map<AlojamientoDto>(AlojamientoUpdate);
            }
            else
            {
                return null;

            }
        }
        public async Task<IEnumerable<AlojamientoDto>> GetAlojamientosByUser(string id)
        {
            var alojamientoByUser = _mapper.Map<IEnumerable<AlojamientoDto>>(await _context.Alojamientos
                .Where(a => a.IdPropietario == id)
                .ToListAsync());
            return alojamientoByUser;
        }

        public Task<IEnumerable<AlojamientoDto>> GetAlojamientosByUser(int id)
        {
            throw new NotImplementedException();
        }
    }
}
