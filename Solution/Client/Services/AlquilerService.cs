using Shared.DataTransferObjects;
using Shared.Services;
using System.Net.Http.Json;
using System.Text.Json;

namespace Client.Services
{
    public class AlquilerService : IAlquilerService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        private readonly string apiUri;

        public AlquilerService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            apiUri = _configuration.GetSection("ApiSettings")["BaseUri"]!;
        }

        public async Task<IEnumerable<AlquilerDto>> GetAllAlquileres()
        {
            var response = await _httpClient.GetAsync($"{apiUri}Alquiler/GetAllAlquileres");

            var result = await Deserialize<List<AlquilerDto>>.DeserializeJson(response);

            return result!;
        }

        public async Task<AlquilerDto> GetAlquiler(int id)
        {
            var response = await _httpClient.GetAsync($"{apiUri}Alquiler/GetAlquiler/{id}");

            var result = await Deserialize<AlquilerDto>.DeserializeJson(response);

            return result!;
        }

        public async Task<AlquilerDto> CreateAlquiler(AlquilerDto alquiler)
        {
            var response = await _httpClient.PostAsJsonAsync($"{apiUri}Alquiler/CreateAlquiler", alquiler);

            var result = await Deserialize<AlquilerDto>.DeserializeJson(response);

            return result!;
        }

        public async Task<AlquilerDto> UpdateAlquiler(AlquilerDto alquiler)
        {
            var response = await _httpClient.PutAsJsonAsync($"{apiUri}Alquiler/UpdateAlquiler", alquiler);

            var result = await Deserialize<AlquilerDto>.DeserializeJson(response);

            return result!;
        }

        public async Task DeleteAlquiler(int id)
        {
            await _httpClient.DeleteAsync($"{apiUri}Alquiler/DeleteAlquiler/{id}");
        }

        public async Task<IEnumerable<AlquilerDto>> GetAlquileresByAlojamiento(int id)
        {
            var response = await _httpClient.GetAsync($"{apiUri}Alquiler/GetAlquileresByAlojamiento/{id}");

            var result = await Deserialize<List<AlquilerDto>>.DeserializeJson(response);

            return result!;
        }

        public async Task<IEnumerable<AlquilerDto>> GetAlquileresByUser(string id)
        {
            var response = await _httpClient.GetAsync($"{apiUri}Alquiler/GetAlquileresByUser/{id}");

            var result = await Deserialize<List<AlquilerDto>>.DeserializeJson(response);

            return result!;
        }
    }
}
