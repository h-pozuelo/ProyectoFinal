using Shared.DataTransferObjects;

namespace Shared.Services
{
    public interface IAlojamientoService
    {

        public Task<IEnumerable<AlojamientoDto>> GetAllAlojamientos();
        public Task<AlojamientoDto> GetAlojamiento(int id);
        public Task<AlojamientoDto> CreateAlojamiento(AlojamientoDto alojamiento);
        public Task<AlojamientoDto> UpdateAlojamiento(AlojamientoDto alojamiento);
        public Task DeleteAlojamiento(int id);

        public Task<IEnumerable<AlojamientoDto>> GetAlojamientosByUser(string id);
    }
}
