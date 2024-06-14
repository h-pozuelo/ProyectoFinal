using Server.Models;

namespace Server.Interfaces
{
    public interface IAlojamientoService
    {

        public Task<IEnumerable<Alojamiento>> GetAllAlojamientos();
        public Task<Alojamiento> GetAlojamiento(int id);
        public Task<Alojamiento> CreateAlojamiento(Alojamiento alojamiento);
        public Task<Alojamiento> UpdateAlojamiento(Alojamiento alojamiento);
        public Task DeleteAlojamiento(int id);

        public Task<IEnumerable<Alojamiento>> GetAlojamientosByUser(int id);

    }
}
