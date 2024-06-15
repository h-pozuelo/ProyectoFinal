using Shared.DataTransferObjects;
using Shared.Services;
using System.Net.Http.Json;
using System.Text.Json;

namespace Client.Services
{
    public class AlojamientoService : IAlojamientoService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        private readonly string apiUri;

        public AlojamientoService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            apiUri = _configuration.GetSection("ApiSettings")["BaseUri"]!;
        }

        public async Task<IEnumerable<AlojamientoDto>> GetAllAlojamientos()
        {
            var response = await _httpClient.GetAsync($"{apiUri}Alojamientos/GetAllAlojamientos");

            var result = await Deserialize<List<AlojamientoDto>>.DeserializeJson(response);

            return result!;
        }

        public async Task<AlojamientoDto> GetAlojamiento(int id)
        {
            var response = await _httpClient.GetAsync($"{apiUri}Alojamientos/GetAlojamiento/{id}");

            var result = await Deserialize<AlojamientoDto>.DeserializeJson(response);

            return result!;
        }

        public async Task<AlojamientoDto> CreateAlojamiento(AlojamientoDto alojamiento)
        {
            var response = await _httpClient.PostAsJsonAsync($"{apiUri}Alojamientos/CreateAlojamiento", alojamiento);

            var result = await Deserialize<AlojamientoDto>.DeserializeJson(response);

            return result!;
        }

        public async Task<AlojamientoDto> UpdateAlojamiento(AlojamientoDto alojamiento)
        {
            var response = await _httpClient.PutAsJsonAsync($"{apiUri}Alojamientos/UpdateAlojamiento", alojamiento);

            var result = await Deserialize<AlojamientoDto>.DeserializeJson(response);

            return result!;
        }

        public async Task DeleteAlojamiento(int id)
        {
            await _httpClient.DeleteAsync($"{apiUri}Alojamientos/DeleteAlojamiento/{id}");
        }

        public async Task<IEnumerable<AlojamientoDto>> GetAlojamientosByUser(string id)
        {
            var response = await _httpClient.GetAsync($"{apiUri}Alojamientos/GetAlojamientosByUser/{id}");

            var result = await Deserialize<List<AlojamientoDto>>.DeserializeJson(response);

            return result!;
        }
    }

    internal static class Deserialize<T>
    {
        public static async Task<T> DeserializeJson(HttpResponseMessage response)
        {
            return JsonSerializer.Deserialize<T>(
                await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
        }
    }
}
