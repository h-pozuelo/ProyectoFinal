using Shared.DataTransferObjects;

namespace Shared.Services
{
    public interface IAlquilerService
    {
        public Task<IEnumerable<AlquilerDto>> GetAllAlquileres();
        public Task<AlquilerDto> GetAlquiler(int id);
        public Task<AlquilerDto> CreateAlquiler(AlquilerDto alquiler);
        public Task<AlquilerDto> UpdateAlquiler(AlquilerDto alquiler);
        public Task DeleteAlquiler(int id);

        public Task<IEnumerable<AlquilerDto>> GetAlquileresByUser(string id);
        public Task<IEnumerable<AlquilerDto>> GetAlquileresByAlojamiento(int id);
    }
}
