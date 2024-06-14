using Shared.DataTransferObjects;
using Shared.Services;
using System.Net.Http.Json;
using System.Text.Json;

namespace Client.Services
{
    public class AlquilerService : IAlquilerService
    {
        private readonly HttpClient _httpClient;

        private readonly string apiUri = "https://localhost:7123/api/";

        public AlquilerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<AlquilerDto>> GetAllAlquileres()
        {
            var response = await _httpClient.GetAsync($"{apiUri}Alquileres/GetAllAlquileres");

            var result = await Deserialize<List<AlquilerDto>>.DeserializeJson(response);

            return result!;
        }

        public async Task<AlquilerDto> GetAlquiler(int id)
        {
            var response = await _httpClient.GetAsync($"{apiUri}Alquileres/GetAlquiler/{id}");

            var result = await Deserialize<AlquilerDto>.DeserializeJson(response);

            return result!;
        }

        public async Task<AlquilerDto> CreateAlquiler(AlquilerDto alquiler)
        {
            var response = await _httpClient.PostAsJsonAsync($"{apiUri}Alquileres/CreateAlquiler", alquiler);

            var result = await Deserialize<AlquilerDto>.DeserializeJson(response);

            return result!;
        }

        public async Task<AlquilerDto> UpdateAlquiler(AlquilerDto alquiler)
        {
            var response = await _httpClient.PutAsJsonAsync($"{apiUri}Alquileres/UpdateAlquiler", alquiler);

            var result = await Deserialize<AlquilerDto>.DeserializeJson(response);

            return result!;
        }

        public async Task DeleteAlquiler(int id)
        {
            await _httpClient.DeleteAsync($"{apiUri}Alquileres/DeleteAlquiler/{id}");
        }

        public async Task<IEnumerable<AlquilerDto>> GetAlquileresByAlojamiento(int id)
        {
            var response = await _httpClient.GetAsync($"{apiUri}Alquileres/GetAlquileresByAlojamiento/{id}");

            var result = await Deserialize<List<AlquilerDto>>.DeserializeJson(response);

            return result!;
        }

        public async Task<IEnumerable<AlquilerDto>> GetAlquileresByUser(string id)
        {
            var response = await _httpClient.GetAsync($"{apiUri}Alquileres/GetAlquileresByUser/{id}");

            var result = await Deserialize<List<AlquilerDto>>.DeserializeJson(response);

            return result!;
        }
    }
}
