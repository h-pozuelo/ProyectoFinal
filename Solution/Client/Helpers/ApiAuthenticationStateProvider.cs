using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace Client.Helpers
{
    // Hereda de la clase "AuthenticationStateProvider"
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        // "HttpClient" es necesario para poder realizar peticiones a la API
        // "ILocalStorageService" es necesario para poder trabajar con el almacenamiento local del navegador
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public ApiAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Recupera del almacenamiento local el valor de aquel elemento con nombre "authToken"
            var savedToken = await _localStorage.GetItemAsync<string>("authToken");

            // Si el valor del elemento "authToken" es nulo o está vacío...
            if (string.IsNullOrEmpty(savedToken)) return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            // Si no es nulo ni está vacío añade la cabecera de autorización "bearer" con dicho valor al "HttpClient"
            // (Con "HttpClient" haremos peticiones a la API por lo que queremos estar autenticados)
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(savedToken), "jwt")));
        }

        public void MarkUserAsAuthenticated(string email)
        {
            var authenticatedUser = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, email)
                    },
                    "apiauth"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void MarkUserAsLoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            // Declaramos una lista de "Claims" que permiten identificar al usuario autenticado
            var claims = new List<Claim>();
            // Separamos el Jwt (utilizando el '.' como separador) para recuperar el "payload" que corresponde al segundo elemento
            var payload = jwt.Split('.')[1];
            // Tranformamos en una lista de bytes la cadena de texto en formato "Base64" ("payload")
            var jsonBytes = ParseBase64WithoutPadding(payload);
            // Deserializa la cadena de texto en formato "JSON" en un diccionario
            var keyValuesPairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            // Intenta recuperar del diccionario aquel "Claim" con nombre "Role"
            // Si el "Claim" con nombre "Role" existe dentro del diccionario almacena su valor en la variable "Object roles"...
            keyValuesPairs!.TryGetValue(ClaimTypes.Role, out object roles);

            if (roles != null)
            {
                // Transforma el objeto "roles" en una cadena de texto quitándole los espacios a sus lados
                // Si la cadena de texto comienza por "["...
                if (roles.ToString()!.Trim().StartsWith("["))
                {
                    // Deserializa la cadena de texto en formato "JSON" en una lista de cadenas de texto
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString()!);

                    foreach (var parsedRole in parsedRoles!)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                // Si la cadena de texto no comienza por "[" quiere decir que solo tiene un rol...
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()!));
                }

                keyValuesPairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(
                keyValuesPairs.Select(x => new Claim(x.Key, x.Value.ToString()!)));

            return claims;
        }

        // Método que devuelve una lista de bytes a partir de la cadena de texto en formato "Base64" recibida como parámetro
        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                // Si la longitud de la cadena de texto entre 4 da como resto 2 debe agregar al final de la cadena "=="...
                case 2: base64 += "=="; break;
                // Si no "="...
                case 3: base64 += "="; break;
            }

            // Devuelve la lista de bytes a partir de la cadena de texto en formato "Base64"
            return Convert.FromBase64String(base64);
        }
    }
}
