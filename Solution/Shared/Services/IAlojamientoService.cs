using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services
{
    public interface IAlojamientoService
    {

        public Task<IEnumerable<Alojamiento>> GetAllAlojamientos();
        public Task<Alojamiento> GetAlojamiento(int id);
        public Task<Alojamiento> CreateAlojamiento(Alojamiento alojamiento);
        public Task<Alojamiento> UpdateAlojamiento(Alojamiento alojamiento);
        public Task DeleteAlojamiento(int id);

    }
}
