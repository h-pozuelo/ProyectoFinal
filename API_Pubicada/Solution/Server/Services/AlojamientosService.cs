using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;
using Shared.DataTransferObjects;
using Shared.Services;
using System.Net;

namespace Server.Services
{
    public class AlojamientosService : IAlojamientosService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AlojamientosService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseDto<IEnumerable<AlojamientoDto>>> GetAllAlojamientos()
        {
            var alojamientos = await _context.Alojamientos.ToListAsync();

            return new ResponseDto<IEnumerable<AlojamientoDto>>
            {
                IsSuccessful = true,
                Element = _mapper.Map<IEnumerable<AlojamientoDto>>(alojamientos),
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ResponseDto<AlojamientoDto>> GetAlojamiento(int idAlojamiento)
        {
            var alojamiento = await _context.Alojamientos.FindAsync(idAlojamiento);

            return new ResponseDto<AlojamientoDto>
            {
                IsSuccessful = true,
                Element = _mapper.Map<AlojamientoDto>(alojamiento),
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ResponseDto<AlojamientoDto>> CreateAlojamiento(AlojamientoDto alojamientoDto)
        {
            var nuevoAlojamiento = _mapper.Map<Alojamiento>(alojamientoDto);

            _context.Alojamientos.Add(nuevoAlojamiento);

            await _context.SaveChangesAsync();

            return new ResponseDto<AlojamientoDto>
            {
                IsSuccessful = true,
                Element = _mapper.Map<AlojamientoDto>(nuevoAlojamiento),
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ResponseDto<AlojamientoDto>> UpdateAlojamiento(AlojamientoDto alojamientoDto)
        {
            var nuevoAlojamiento = await _context.Alojamientos.FindAsync(alojamientoDto.Id);

            if (nuevoAlojamiento != null)
            {
                _mapper.Map<AlojamientoDto, Alojamiento>(alojamientoDto, nuevoAlojamiento);

                await _context.SaveChangesAsync();
            }

            return new ResponseDto<AlojamientoDto>
            {
                IsSuccessful = true,
                Element = _mapper.Map<AlojamientoDto>(nuevoAlojamiento),
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ResponseDto<AlojamientoDto>> DeleteAlojamiento(int idAlojamiento)
        {
            var alojamientoBorrar = await _context.Alojamientos.FindAsync(idAlojamiento);

            if (alojamientoBorrar != null)
            {
                _context.Alojamientos.Remove(alojamientoBorrar);

                await _context.SaveChangesAsync();
            }

            return new ResponseDto<AlojamientoDto>
            {
                IsSuccessful = true,
                Element = _mapper.Map<AlojamientoDto>(alojamientoBorrar),
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ResponseDto<IEnumerable<AlojamientoDto>>> GetAllAlojamientosPropietario(string idPropietario)
        {
            var alojamientos = await _context.Alojamientos
                .Where(al => al.IdPropietario == idPropietario).ToListAsync();

            return new ResponseDto<IEnumerable<AlojamientoDto>>
            {
                IsSuccessful = true,
                Element = _mapper.Map<IEnumerable<AlojamientoDto>>(alojamientos),
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}
