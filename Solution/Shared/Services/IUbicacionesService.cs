using Shared.DataTransferObjects;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services
{
    public interface IUbicacionesService
    {
        public Task<ResponseDto<IEnumerable<Comunidad>>> GetComunidades();
        public Task<ResponseDto<IEnumerable<Provincia>>> GetProvincias();
        public Task<ResponseDto<IEnumerable<Ciudad>>> GetCiudades();
        public Task<ResponseDto<IEnumerable<Provincia>>> GetProvinciasComunidad(string idComunidad);
        public Task<ResponseDto<IEnumerable<Ciudad>>> GetCiudadesProvincia(string idProvincia);
    }
}
