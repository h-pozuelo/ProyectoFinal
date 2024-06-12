using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services
{
    public interface IAlquilerService
    {
        public Task<IEnumerable<Alquiler>> GetAllAlquileres();
        public Task<Alquiler> GetAlquiler(int id);
        public Task<Alquiler> CreateAlquiler(Alquiler alquiler);
        public Task<Alquiler> UpdateAlquiler(Alquiler alquiler);
        public Task DeleteAlquiler(int id);
    }
}
