using Shared.DataTransferObjects;

namespace Shared.Services
{
    public interface IAlojamientosService
    {
        public Task<ResponseDto<IEnumerable<AlojamientoDto>>> GetAllAlojamientos();
        public Task<ResponseDto<AlojamientoDto>> GetAlojamiento(int idAlojamiento);
        public Task<ResponseDto<AlojamientoDto>> CreateAlojamiento(AlojamientoDto alojamientoDto);
        public Task<ResponseDto<AlojamientoDto>> UpdateAlojamiento(AlojamientoDto alojamientoDto);
        public Task<ResponseDto<AlojamientoDto>> DeleteAlojamiento(int idAlojamiento);

        public Task<ResponseDto<IEnumerable<AlojamientoDto>>> GetAllAlojamientosPropietario(string idPropietario);
    }
}
