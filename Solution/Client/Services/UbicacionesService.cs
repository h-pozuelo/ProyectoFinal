using Shared.DataTransferObjects;
using Shared.Models;
using Shared.Services;

namespace Client.Services
{
    public class UbicacionesService : IUbicacionesService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        private readonly string apiUri;

        public UbicacionesService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            apiUri = _configuration.GetSection("ApiSettings")["BaseUri"]!;
        }

        public async Task<ResponseDto<IEnumerable<Comunidad>>> GetComunidades()
        {
            var response = await _httpClient.GetAsync($"{apiUri}Ubicaciones/GetComunidades");

            var result = await Deserialize<ResponseDto<IEnumerable<Comunidad>>>
                .DeserializeJson(response);

            return result!;
        }

        public async Task<ResponseDto<IEnumerable<Provincia>>> GetProvincias()
        {
            var response = await _httpClient.GetAsync($"{apiUri}Ubicaciones/GetProvincias");

            var result = await Deserialize<ResponseDto<IEnumerable<Provincia>>>
                .DeserializeJson(response);

            return result!;
        }

        public async Task<ResponseDto<IEnumerable<Ciudad>>> GetCiudades()
        {
            var response = await _httpClient.GetAsync($"{apiUri}Ubicaciones/GetCiudades");

            var result = await Deserialize<ResponseDto<IEnumerable<Ciudad>>>
                .DeserializeJson(response);

            return result!;
        }

        public async Task<ResponseDto<IEnumerable<Provincia>>> GetProvinciasComunidad(string idComunidad)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{apiUri}Ubicaciones/GetProvinciasComunidad");

            request.Headers.Add("idComunidad", idComunidad);

            var response = await _httpClient.SendAsync(request);

            var result = await Deserialize<ResponseDto<IEnumerable<Provincia>>>
                .DeserializeJson(response);

            return result!;
        }

        public async Task<ResponseDto<IEnumerable<Ciudad>>> GetCiudadesProvincia(string idProvincia)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{apiUri}Ubicaciones/GetCiudadesProvincia");

            request.Headers.Add("idProvincia", idProvincia);

            var response = await _httpClient.SendAsync(request);

            var result = await Deserialize<ResponseDto<IEnumerable<Ciudad>>>
                .DeserializeJson(response);

            return result!;
        }
    }
}
