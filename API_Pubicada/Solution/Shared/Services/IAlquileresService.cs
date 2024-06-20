using Shared.DataTransferObjects;

namespace Shared.Services
{
    public interface IAlquileresService
    {
        public Task<ResponseDto<IEnumerable<AlquilerDto>>> GetAllAlquileres();
        public Task<ResponseDto<AlquilerDto>> GetAlquiler(int idAlquiler);
        public Task<ResponseDto<AlquilerDto>> CreateAlquiler(AlquilerDto alquilerDto);
        public Task<ResponseDto<AlquilerDto>> UpdateAlquiler(AlquilerDto alquilerDto);
        public Task<ResponseDto<AlquilerDto>> DeleteAlquiler(int idAlquiler);

        public Task<ResponseDto<IEnumerable<AlquilerDto>>> GetAllAlquileresAlojamiento(int idAlojamiento);
        public Task<ResponseDto<IEnumerable<AlquilerDto>>> GetAllAlquileresInquilino(string idInquilino);
    }
}
