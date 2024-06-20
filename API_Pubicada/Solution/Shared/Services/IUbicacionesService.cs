using Shared.DataTransferObjects;
using Shared.Models;

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
