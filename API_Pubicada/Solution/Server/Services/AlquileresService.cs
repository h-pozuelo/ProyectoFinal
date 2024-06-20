using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;
using Shared.DataTransferObjects;
using Shared.Services;
using System.Net;

namespace Server.Services
{
    public class AlquileresService : IAlquileresService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AlquileresService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseDto<IEnumerable<AlquilerDto>>> GetAllAlquileres()
        {
            var alquileres = await _context.Alquileres.ToListAsync();

            return new ResponseDto<IEnumerable<AlquilerDto>>
            {
                IsSuccessful = true,
                Element = _mapper.Map<IEnumerable<AlquilerDto>>(alquileres),
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ResponseDto<AlquilerDto>> GetAlquiler(int idAlquiler)
        {
            var alquiler = await _context.Alquileres.FindAsync(idAlquiler);

            return new ResponseDto<AlquilerDto>
            {
                IsSuccessful = true,
                Element = _mapper.Map<AlquilerDto>(alquiler),
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ResponseDto<AlquilerDto>> CreateAlquiler(AlquilerDto alquilerDto)
        {
            var nuevoAlquiler = _mapper.Map<Alquiler>(alquilerDto);

            _context.Alquileres.Add(nuevoAlquiler);

            await _context.SaveChangesAsync();

            return new ResponseDto<AlquilerDto>
            {
                IsSuccessful = true,
                Element = _mapper.Map<AlquilerDto>(nuevoAlquiler),
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ResponseDto<AlquilerDto>> UpdateAlquiler(AlquilerDto alquilerDto)
        {
            var nuevoAlquiler = await _context.Alquileres.FindAsync(alquilerDto.Id);

            if (nuevoAlquiler != null)
            {
                _mapper.Map<AlquilerDto, Alquiler>(alquilerDto, nuevoAlquiler);

                await _context.SaveChangesAsync();
            }

            return new ResponseDto<AlquilerDto>
            {
                IsSuccessful = true,
                Element = _mapper.Map<AlquilerDto>(nuevoAlquiler),
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ResponseDto<AlquilerDto>> DeleteAlquiler(int idAlquiler)
        {
            var alquilerBorrar = await _context.Alquileres.FindAsync(idAlquiler);

            if (alquilerBorrar != null)
            {
                _context.Alquileres.Remove(alquilerBorrar);

                await _context.SaveChangesAsync();
            }

            return new ResponseDto<AlquilerDto>
            {
                IsSuccessful = true,
                Element = _mapper.Map<AlquilerDto>(alquilerBorrar),
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ResponseDto<IEnumerable<AlquilerDto>>> GetAllAlquileresAlojamiento(int idAlojamiento)
        {
            var alquileres = await _context.Alquileres
                .Where(a => a.IdAlojamiento == idAlojamiento).ToListAsync();

            return new ResponseDto<IEnumerable<AlquilerDto>>
            {
                IsSuccessful = true,
                Element = _mapper.Map<IEnumerable<AlquilerDto>>(alquileres),
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ResponseDto<IEnumerable<AlquilerDto>>> GetAllAlquileresInquilino(string idInquilino)
        {
            var alquileres = await _context.Alquileres
                .Where(a => a.IdInquilino == idInquilino).ToListAsync();

            return new ResponseDto<IEnumerable<AlquilerDto>>
            {
                IsSuccessful = true,
                Element = _mapper.Map<IEnumerable<AlquilerDto>>(alquileres),
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}
