using Blazored.LocalStorage;
using Shared.DataTransferObjects;
using Shared.Services;
using System.Text;
using System.Text.Json;

namespace Client.Services
{
    public class UsuariosService : IUsuariosService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;
        private readonly IConfiguration _configuration;

        private readonly string apiUri;

        public UsuariosService(HttpClient httpClient, ILocalStorageService localStorageService, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
            _configuration = configuration;
            apiUri = _configuration.GetSection("ApiSettings")["BaseUri"]!;
        }

        public async Task<ResponseDto<UserForUpdateDto>> GetUsuario(string idUsuario)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{apiUri}Usuarios/GetUsuario");

            request.Headers.Add("id", idUsuario);

            var response = await _httpClient.SendAsync(request);

            var result = JsonSerializer.Deserialize<ResponseDto<UserForUpdateDto>>(
                await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return result!;
        }

        public async Task<ResponseDto<UserForUpdateDto>> UpdateUsuario(UserForUpdateDto usuario, string idUsuario)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"{apiUri}Usuarios/UpdateUsuario");

            request.Headers.Add("id", idUsuario);

            var usuarioAsJson = JsonSerializer.Serialize(usuario);

            request.Content = new StringContent(usuarioAsJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            var result = JsonSerializer.Deserialize<ResponseDto<UserForUpdateDto>>(
                await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return result!;
        }

        public async Task<ResponseDto<PasswordForUpdateDto>> UpdatePassword(PasswordForUpdateDto password, string idUsuario)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"{apiUri}Usuarios/UpdatePassword");

            request.Headers.Add("id", idUsuario);

            var passwordAsJson = JsonSerializer.Serialize(password);

            request.Content = new StringContent(passwordAsJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            var result = JsonSerializer.Deserialize<ResponseDto<PasswordForUpdateDto>>(
                await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return result!;
        }

        public async Task<ResponseDto<string>> GetNombreUsuario(string idUsuario)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{apiUri}Usuarios/GetNombreUsuario/{idUsuario}");

            var response = await _httpClient.SendAsync(request);

            var result = JsonSerializer.Deserialize<ResponseDto<string>>(
                await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return result!;
        }
    }
}
