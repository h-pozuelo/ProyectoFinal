using Blazored.LocalStorage;
using ClassLibrary.DataTransferObjects;
using Client.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Client.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _stateProvider;
        private readonly ILocalStorageService _localStorage;

        private readonly string apiUri = "https://localhost:7123/api/";

        public AuthenticationService(HttpClient httpClient, AuthenticationStateProvider stateProvider, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _stateProvider = stateProvider;
            _localStorage = localStorage;
        }

        public async Task<RegistrationResponseDto> Register(UserForRegistrationDto registerModel)
        {
            var result = await _httpClient.PostAsJsonAsync($"{apiUri}Accounts/Registration", registerModel);

            if (!result.IsSuccessStatusCode) return new RegistrationResponseDto
            {
                IsSuccessfulRegistration = false,
                Errors = new List<string> { "Ha ocurrido un error." }
            };

            return new RegistrationResponseDto
            {
                IsSuccessfulRegistration = true,
                Errors = new List<string> { "Cuenta creada correctamente." }
            };
        }

        public async Task<AuthenticationResponseDto> Login(UserForAuthenticationDto loginModel)
        {
            var loginAsJson = JsonSerializer.Serialize(loginModel);
            var response = await _httpClient.PostAsync(
                $"{apiUri}Accounts/Login",
                new StringContent(loginAsJson, Encoding.UTF8, "application/json"));
            var loginResult = JsonSerializer.Deserialize<AuthenticationResponseDto>(
                await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (!response.IsSuccessStatusCode) return loginResult!;

            await _localStorage.SetItemAsync("authToken", loginResult!.Token);

            //((ApiAuthenticationStateProvider)_stateProvider).MarkUserAsAuthenticated(loginModel.Email!);

            ((ApiAuthenticationStateProvider)_stateProvider).MarkUserAsAuthenticated(loginResult.Token!);

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("bearer", loginResult.Token);

            return loginResult!;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");

            ((ApiAuthenticationStateProvider)_stateProvider).MarkUserAsLoggedOut();

            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
