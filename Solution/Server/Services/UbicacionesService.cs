using Microsoft.Azure.Cosmos;
using Shared.DataTransferObjects;
using Shared.Models;
using Shared.Services;
using System.Net;

namespace Server.Services
{
    public class UbicacionesService : IUbicacionesService
    {
        private readonly CosmosClient _client;
        private readonly Container _container;

        private IEnumerable<Comunidad>? _comunidades;
        private IEnumerable<Provincia>? _provincias;
        private IEnumerable<Ciudad>? _ciudades;

        public UbicacionesService(CosmosClient client, IConfiguration configuration)
        {
            _client = client;
            var cosmosDb = configuration.GetSection("CosmosDb");
            _container = _client.GetContainer(
                cosmosDb["DatabaseName"],
                cosmosDb["ContainerName"]);
        }

        public async Task<ResponseDto<IEnumerable<Comunidad>>> GetComunidades()
        {
            if (_comunidades == null || _comunidades.Count() == 0)
            {
                List<Comunidad> comunidades = new List<Comunidad>();

                var query = "SELECT c.parent_code, c.label, c.code FROM c";
                var iterator = _container.GetItemQueryIterator<Comunidad>(
                    new QueryDefinition(query));

                while (iterator.HasMoreResults)
                {
                    var response = await iterator.ReadNextAsync();
                    comunidades.AddRange(response.ToList());
                }

                _comunidades = comunidades;
            }

            return new ResponseDto<IEnumerable<Comunidad>>
            {
                IsSuccessful = true,
                Element = _comunidades,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ResponseDto<IEnumerable<Provincia>>> GetProvincias()
        {
            if (_provincias == null || _provincias.Count() == 0)
            {
                List<Provincia> provincias = new List<Provincia>();

                var query = "SELECT p.parent_code, p.code, p.label FROM c JOIN p IN c.provinces";

                var iterator = _container.GetItemQueryIterator<Provincia>(
                    new QueryDefinition(query));

                while (iterator.HasMoreResults)
                {
                    var response = await iterator.ReadNextAsync();
                    provincias.AddRange(response.ToList());
                }

                _provincias = provincias;
            }

            return new ResponseDto<IEnumerable<Provincia>>
            {
                IsSuccessful = true,
                Element = _provincias,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ResponseDto<IEnumerable<Ciudad>>> GetCiudades()
        {
            if (_ciudades == null || _ciudades.Count() == 0)
            {
                List<Ciudad> ciudades = new List<Ciudad>();

                var query = "SELECT t.parent_code, t.code, t.label FROM c JOIN p IN c.provinces JOIN t IN p.towns";

                var iterator = _container.GetItemQueryIterator<Ciudad>(
                    new QueryDefinition(query));

                while (iterator.HasMoreResults)
                {
                    var response = await iterator.ReadNextAsync();
                    ciudades.AddRange(response.ToList());
                }

                _ciudades = ciudades;
            }

            return new ResponseDto<IEnumerable<Ciudad>>
            {
                IsSuccessful = true,
                Element = _ciudades,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ResponseDto<IEnumerable<Provincia>>> GetProvinciasComunidad(string idComunidad)
        {
            if (_provincias == null || _provincias.Count() == 0)
            {
                await GetProvincias();
            }

            List<Provincia> results = _provincias!
                .Where(p => p.IdComunidad == idComunidad).ToList();

            return new ResponseDto<IEnumerable<Provincia>>
            {
                IsSuccessful = true,
                Element = results,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ResponseDto<IEnumerable<Ciudad>>> GetCiudadesProvincia(string idProvincia)
        {
            if (_ciudades == null || _ciudades.Count() == 0)
            {
                await GetCiudades();
            }

            List<Ciudad> results = _ciudades!
                .Where(t => t.IdProvincia == idProvincia).ToList();

            return new ResponseDto<IEnumerable<Ciudad>>
            {
                IsSuccessful = true,
                Element = results,
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}
