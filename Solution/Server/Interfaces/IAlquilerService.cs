using Server.Models;

namespace Server.Interfaces
{
    public interface IAlquilerService
    {
        public Task<IEnumerable<Alquiler>> GetAllAlquileres();
        public Task<Alquiler> GetAlquiler(int id);
        public Task<Alquiler> CreateAlquiler(Alquiler alquiler);
        public Task<Alquiler> UpdateAlquiler(Alquiler alquiler);
        public Task DeleteAlquiler(int id);

        public Task<IEnumerable<Alquiler>> GetAlquileresByUser(int id);
        public Task<IEnumerable<Alquiler>> GetAlquileresByAlojamiento(int id);
    }
}
