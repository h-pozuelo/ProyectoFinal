Dentro de Visual Studio 2022 crear una solución en blanco > "Solution" :

	- Crear una aplicación de tipo "Aplicación Blazor para WebAssembly" > "Client"

	- Crear una aplicación de tipo "ASP.NET Core Web API" > "Server" :

		- Microsoft.AspNetCore.Authentication.JwtBearer
		- Microsoft.AspNetCore.Identity.UI
		- Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
		- Microsoft.EntityFrameworkCore.SqlServer
		- Microsoft.EntityFrameworkCore.Tools
		- Microsoft.AspNetCore.Identity.EntityFrameworkCore

		- "appsettings.json" :
```
{
  ...,
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=development;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"
  }
}
```

		- Crear la carpeta "~/Data/" :
			
			- "ApplicationDbContext.cs" :
```
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Server.Data
{
    // Hereda de "IdentityDbContext" en vez de "DbContext" dado que vamos a implementar autenticación
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    }
}
```

		- "Program.cs" :
```
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server.Data;
...
// Recupera del fichero "~/appsettings.json" la cadena de conexión "DefaultConnection" para ser utilizada en nuestro contexto de base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// Servicio que crea tanto la tabla de "AspNetRoles" como la tabla de "AspNetUsers"
builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
...
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Cuando se encuentra en un entorno de desarrollo crea la base de datos
    var db = app.Services
        .CreateScope()
        .ServiceProvider.GetService<ApplicationDbContext>();
    db?.Database.EnsureCreated();
    ...
}
...
```

        - "Server.csproj" :
```
<Project Sdk="Microsoft.NET.Sdk.Web">

	<PropertyGroup>
		...
		<InvariantGlobalization>false</InvariantGlobalization>
	</PropertyGroup>
	...
</Project>
```

		- "Program.cs" :
```
using Microsoft.AspNetCore.Authentication.JwtBearer;
...
using Microsoft.IdentityModel.Tokens;
...
using System.Text;
...
// Servicio que configura Jwt en el servidor
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["JwtIssuer"],
            ValidAudience = jwtSettings["JwtAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.GetValue<string>("JwtSecurityKey")!))
        };
    });
...
// Habilita la autenticación en el servidor
// Obligatorio ponerlo antes de "app.UseAuthorization();"
app.UseAuthentication();
app.UseAuthorization();
...
```

        - "appsettings.json" :
```
{
  ...,
  "JwtSettings": {
    "JwtSecurityKey": "kBktpQ6etkEaDS1rAjMy9sdw40dPAXnZK1jJg2B3NPeax4En0STQcAExDkU90DDh",
    "JwtIssuer": "https://localhost",
    "JwtAudience": "https://localhost",
    "JwtExpiryInDays": 1
  }
}
```

        - Crear la carpeta "~/DataTransferObjects/" :

            - "UserForRegistrationDto.cs" :
```
using System.ComponentModel.DataAnnotations;

namespace Server.DataTransferObjects
{
    public class UserForRegistrationDto
    {
        [Required(ErrorMessage = "El nombre es requerido."),
            DataType(DataType.Text), Display(Name = "Nombre")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "El correo es requerido."),
            EmailAddress, Display(Name = "Correo")]
        public string? Email { get; set; }

        //[Required(ErrorMessage = "El usuario es requerido."),
        //    DataType(DataType.Text), Display(Name = "Usuario")]
        //public string? UserName { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida."),
            DataType(DataType.Password), Display(Name = "Contraseña"),
            StringLength(100, ErrorMessage = "La {0} debe tener una longitud de entre {2} y {1} caracteres.", MinimumLength = 6)]
        public string? Password { get; set; }

        [DataType(DataType.Password), Display(Name = "Confirmar contraseña"),
            Compare(nameof(Password), ErrorMessage = "La contraseña no coincide.")]
        public string? ConfirmPassword { get; set; }
    }

    public class RegistrationResponseDto
    {
        public bool IsSuccessfulRegistration { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
```

            - "UserForAuthenticationDto.cs :
```
using System.ComponentModel.DataAnnotations;

namespace Server.DataTransferObjects
{
    public class UserForAuthenticationDto
    {
        [Required(ErrorMessage = "El correo es requerido."),
            EmailAddress, Display(Name = "Correo")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida."),
            DataType(DataType.Password), Display(Name = "Contraseña")]
        public string? Password { get; set; }
    }

    public class AuthenticationResponseDto
    {
        public bool IsAuthenticationSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Token { get; set; }
    }
}
```

        - Creamos la carpeta "~/Models/" :
            
            - "Usuario.cs" :
```
using Microsoft.AspNetCore.Identity;

namespace Server.Models
{
    // Hereda de la clase "IdentityUser" por que queremos agregar propiedades adicionales al momento de construir la tabla "AspNetUsers"
    public class Usuario : IdentityUser
    {
        public string? NombreCompleto { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
```

        - AutoMapper

        - Creamos el fichero "~/MappingProfile.cs" :
```
using AutoMapper;
using Server.DataTransferObjects;
using Server.Models;

namespace Server
{
    // Hereda de la clase "AutoMapper.Profile"
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserForRegistrationDto, Usuario>()
                // Mapea el valor de "UserForRegistrationDto.FullName" a "Usuario.NombreCompleto"
                .ForMember(u => u.NombreCompleto, opt => opt.MapFrom(x => x.FullName))
                // Mapea el valor de "UserForRegistrationDto.Email" a "Usuario.Username"
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
        }
    }
}
```

        - "Program.cs" :
```
...
using Server;
...
// Servicio que configura AutoMapper con el constructor de la clase "MappingProfile"
builder.Services.AddAutoMapper(typeof(MappingProfile));
...
```

        - Creamos la carpeta "~/Configuration/" :

            - "RoleConfiguration.cs" :
```
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Server.Configuration
{
    // Hereda de la clase "IEntityTypeConfiguration" del tipo "IdentityRole"
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        // Método para agregar registros iniciales al momento de construir la tabla "AspNetRoles"
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Name = "Visitor",
                    NormalizedName = "VISITOR"
                },
                new IdentityRole
                {
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                });
        }
    }
}
```

        - Dentro de "~/Data/" :

            - "ApplicationDbContext.cs" :
```
...
using Server.Configuration;
using Server.Models;

namespace Server.Data
{
    // Hereda de "IdentityDbContext" en vez de "DbContext" dado que vamos a implementar autenticación
    public class ApplicationDbContext : IdentityDbContext<Usuario>
    {
        ...
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new RoleConfiguration());
        }
    }
}
```

        - "Program.cs" :
```
...
using Server.Models;
...
// Servicio que crea tanto la tabla de "AspNetRoles" como la tabla de "AspNetUsers"
builder.Services.AddIdentity<Usuario, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<ApplicationDbContext>();
...
```

        - Dentro de "~/Controllers/" crear un controlador de tipo "Controlador de API: en blanco" > "AccountsController.cs" :
```
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.DataTransferObjects;
using Server.Models;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<Usuario> _userManager;
        // Necesario para mapear
        private readonly IMapper _mapper;

        public AccountsController(UserManager<Usuario> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto model)
        {
            if (model == null || !ModelState.IsValid) return BadRequest();

            // Se encarga de crear un "Usuario" a partir del modelo "UserForRegistrationDto" recibido como parámetro
            // Mapea el valor de "UserForRegistrationDto.FullName" a "Usuario.NombreCompleto"
            // Mapea el valor de "UserForRegistrationDto.Email" a "Usuario.UserName"
            var newUser = _mapper.Map<Usuario>(model);

            newUser.FechaRegistro = DateTime.Now;

            var result = await _userManager.CreateAsync(newUser, model.Password!);

            if (result.Succeeded) return Ok(new RegistrationResponseDto { IsSuccessfulRegistration = true });

            var errors = result.Errors.Select(x => x.Description);

            return BadRequest(new RegistrationResponseDto { IsSuccessfulRegistration = false, Errors = errors });
        }
    }
}
```

        - Creamos la carpeta "~/JwtFeatures/" :
            
            - "JwtHandler.cs" :
```
using Microsoft.IdentityModel.Tokens;
using Server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.JwtFeatures
{
    public class JwtHandler
    {
        // Necesario para recuperar la configuración definida en el fichero "appsettings.json"
        private readonly IConfiguration _configuration;
        private readonly IConfiguration _jwtSettings;

        public JwtHandler(IConfiguration configuration)
        {
            _configuration = configuration;
            _jwtSettings = _configuration.GetSection("JwtSettings");
        }

        // Método que genera credenciales de acceso
        public SigningCredentials GetSigningCredentials()
        {
            // Recupera del fichero "appsettings.json" la clave de seguridad de Jwt
            var key = Encoding.UTF8.GetBytes(_jwtSettings["JwtSecurityKey"]!);
            // Crea un secreto a partir de los bytes recuperados de la clave de seguridad de Jwt
            var secret = new SymmetricSecurityKey(key);

            // Devuelve las credenciales generadas a partir del secreto con el algoritmo "HS256"
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        // Método que recupera los "Claims" del usuario recibido como parámetro
        public List<Claim> GetClaims(Usuario user)
        {
            var claims = new List<Claim>
            {
                new Claim("Id", user.Id),
                new Claim("Email", user.Email!),
                new Claim("UserName", user.UserName!),
                new Claim("NombreCompleto", user.NombreCompleto!),
                new Claim("FechaRegistro", user.FechaRegistro.ToString()),
            };

            return claims;
        }

        // Método que genera opciones del token de Jwt a partir de las credenciales de acceso / lista de "Claims" recibidos como parámetros
        public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var expiry = DateTime.Now.AddDays(Convert.ToInt32(_jwtSettings["JwtExpiryInDays"]));
            var tokenOptions = new JwtSecurityToken(
                issuer: _jwtSettings["JwtIssuer"],
                audience: _jwtSettings["JwtAudience"],
                claims: claims,
                expires: expiry,
                signingCredentials: signingCredentials);

            return tokenOptions;
        }
    }
}
```

        - "Program.cs" :
```
...
using Server.JwtFeatures;
...
// Servicio que proporciona a la clase "JwtHandler" como dependencia en el servidor
builder.Services.AddScoped<JwtHandler>();
...
```

        - "~/Controllers/AccountsController.cs" :
```
...
using Server.JwtFeatures;
...
using System.IdentityModel.Tokens.Jwt;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        ...
        // Necesario para autenticar usuarios en el servidor
        private readonly SignInManager<Usuario> _signInManager;
        // Necesario para recuperar las opciones de Jwt configuradas en el servidor
        private readonly JwtHandler _jwtHandler;

        public AccountsController(UserManager<Usuario> userManager, IMapper mapper, SignInManager<Usuario> signInManager, JwtHandler jwtHandler)
        {
            ...
            // Gestiona credenciales del tipo "IdentityUser<Usuario>"
            _signInManager = signInManager;
            _jwtHandler = jwtHandler;
        }
        ...
        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser([FromBody] UserForAuthenticationDto model)
        {
            // Si el modelo recibido como parámetro es nulo o no es válido...
            if (model == null || !ModelState.IsValid) return BadRequest();

            Usuario? user = await _userManager.FindByEmailAsync(model.Email!);

            // Si no existe ningún usuario en la base de datos con ese correo...
            if (user == null) return NotFound();

            var result = await _signInManager.PasswordSignInAsync(user, model.Password!, false, false);

            // Si las credenciales de autenticación no son válidas...
            if (!result.Succeeded) return Unauthorized(new AuthenticationResponseDto { ErrorMessage = "Credenciales incorrectas." });

            // Recupera la lista de "Claims" correspondiente al usuario recuperado de la base de datos
            // Genera las credenciales de acceso
            // Recupera las opciones del token de Jwt a partir de las credenciales de acceso / lista de "Claims"
            var claims = _jwtHandler.GetClaims(user);
            var signingCredentials = _jwtHandler.GetSigningCredentials();
            var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return Ok(new AuthenticationResponseDto { IsAuthenticationSuccessful = true, Token = token });
        }
    }
}
```

        - "Program.cs" :
```
...
// Servicio que condigura CORS en el servidor
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "MyPolicy",
        configurePolicy: builder =>
        {
            builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
        });
});
...
// Debemos incluir el uso de CORS entre "app.UseRouting();" ... "app.UseCors();" ... "app.UseAuthentication();"
app.UseCors(policyName: "MyPolicy");
...
```

        - "~/Controllers/AccountsController.cs" :
```
...
namespace Server.Controllers
{
    [EnableCors(policyName: "MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        ...
    }
}
```

	- Crear una "Biblioteca de clases" > "ClassLibrary" :

        - Creamos la carpeta "~/DataTransferObjects/" :

            - "UserForRegistrationDto.cs"
            - "UserForAuthenticationDto.cs"

    - Cambiar todas las referencias :

        - "Server/MappingProfile.cs"
        - "Server/Controllers/AccountsController.cs"

    - "Client" :

        - Blazored.LocalStorage
        - Microsoft.AspNetCore.Components.Authorization

        - "App.razor" :
```
@using Microsoft.AspNetCore.Components.Authorization
...
<Router AppAssembly="@typeof(Program).Assembly">
    <Found Context="routeData">
        <AuthorizeRouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" />
    </Found>
    <NotFound>
        <CascadingAuthenticationState>
            <PageTitle>Not found</PageTitle>
            <LayoutView Layout="@typeof(MainLayout)">
                <p role="alert">Sorry, there's nothing at this address.</p>
            </LayoutView>
        </CascadingAuthenticationState>
    </NotFound>
</Router>
```

        - Creamos la carpeta "~/Helpers/" :

            - "ApiAuthenticationStateProvider.cs" :
```
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
```

        - Creamos la carpeta "~/Services/" :

            - "IAuthenticationService.cs" :
```
using ClassLibrary.DataTransferObjects;

namespace Client.Services
{
    public interface IAuthenticationService
    {
        Task<RegistrationResponseDto> Register(UserForRegistrationDto registerModel);
        Task<AuthenticationResponseDto> Login(UserForAuthenticationDto loginModel);
        Task Logout();
    }
}
```

            - "AuthenticationService.cs" :
```
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

            ((ApiAuthenticationStateProvider)_stateProvider).MarkUserAsAuthenticated(loginModel.Email!);

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
```

        - Dentro de "~/Pages/" :

            - "Login.razor" :
```
@page "/login"
@using ClassLibrary.DataTransferObjects
@using Services
@inject IAuthenticationService AuthenticationService
@inject NavigationManager NavigationManager

<h3>Login</h3>

@if (showErrors)
{
    <div class="alert alert-danger" role="alert">
        <p>@error</p>
    </div>
}

<div class="card">
    <div class="card-body">
        <h5 class="card-title">Rellena el formulario.</h5>
        <EditForm Model="loginModel" OnValidSubmit="HandleLogin">
            <DataAnnotationsValidator />
            <ValidationSummary />

            <div class="form-group">
                <label for="email">Correo</label>
                <InputText id="email" class="form-control" @bind-Value="loginModel.Email" />
                <ValidationMessage For="@(() => loginModel.Email)" />
            </div>
            <div class="form-group">
                <label for="password">Contraseña</label>
                <InputText id="password" type="password" class="form-control" @bind-Value="loginModel.Password" />
                <ValidationMessage For="@(() => loginModel.Password)" />
            </div>
            <button type="submit" class="btn btn-primary">Enviar</button>
        </EditForm>
    </div>
</div>

@code {
    private UserForAuthenticationDto loginModel = new UserForAuthenticationDto();
    private bool showErrors;
    private string error = "";

    private async Task HandleLogin()
    {
        showErrors = false;

        var result = await AuthenticationService.Login(loginModel);

        if (result.IsAuthenticationSuccessful) NavigationManager.NavigateTo("/");

        error = result.ErrorMessage!;
        showErrors = true;
    }
}
```

        - "Program.cs" :
```
using Blazored.LocalStorage;
...
using Client.Helpers;
using Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
...
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
...
```

        - "App.razor" :
```
...
@code {
    [CascadingParameter]
    private Task<AuthenticationState> authenticationStateTask { get; set; }

    private async Task LogUserAuthenticationState()
    {
        var authState = await authenticationStateTask;
        var user = authState.User;

        if (user.Identity!.IsAuthenticated) Console.WriteLine($"El usuario {user.Identity.Name} se encuentra autenticado.");
        else Console.WriteLine("El usuario no se encuentra autenticado");
    }
}
```

        - Dentro de "~/Pages/" :
            
            - "Register.razor" :
```
@page "/register"
@using ClassLibrary.DataTransferObjects
@using Services
@inject IAuthenticationService AuthenticationService
@inject NavigationManager NavigationManager

<h3>Register</h3>

@if (showErrors)
{
    <div class="alert alert-danger" role="alert">
        @foreach (var error in errors!)
        {
            <p>@error</p>
        }
    </div>
}

<div class="card">
    <div class="card-body">
        <h5 class="card-title">Rellena el formulario.</h5>
        <EditForm Model="registerModel" OnValidSubmit="HandleRegistration">
            <DataAnnotationsValidator />
            <ValidationSummary />

            <div class="form-group">
                <label for="nombreCompleto">Nombre Completo</label>
                <InputText id="nombreCompleto" class="form-control" @bind-Value="registerModel.FullName" />
                <ValidationMessage For="@(() => registerModel.FullName)" />
            </div>
            <div class="form-group">
                <label for="email">Correo</label>
                <InputText id="email" class="form-control" @bind-Value="registerModel.Email" />
                <ValidationMessage For="@(() => registerModel.Email)" />
            </div>
            <div class="form-group">
                <label for="password">Contraseña</label>
                <InputText id="password" type="password" class="form-control" @bind-Value="registerModel.Password" />
                <ValidationMessage For="@(() => registerModel.Password)" />
            </div>
            <div class="form-group">
                <label for="confirmPassword">Confirmar Contraseña</label>
                <InputText id="confirmPassword" type="password" class="form-control" @bind-Value="registerModel.ConfirmPassword" />
                <ValidationMessage For="@(() => registerModel.ConfirmPassword)" />
            </div>
            <button type="submit" class="btn btn-primary">Enviar</button>
        </EditForm>
    </div>
</div>

@code {
    private UserForRegistrationDto registerModel = new UserForRegistrationDto();
    private bool showErrors;
    private IEnumerable<string>? errors;

    private async Task HandleRegistration()
    {
        showErrors = false;

        var result = await AuthenticationService.Register(registerModel);

        if (result.IsSuccessfulRegistration) NavigationManager.NavigateTo("/login");

        errors = result.Errors;
        showErrors = true;
    }
}
```

            - "Logout.cs" :
```
@page "/logout"
@using Services
@inject IAuthenticationService AuthenticationService
@inject NavigationManager NavigationManager

@code {
    protected override async Task OnInitializedAsync()
    {
        await AuthenticationService.Logout();
        NavigationManager.NavigateTo("/");
    }
}
```

            - "LoginDisplay.cs" :
```
@using Microsoft.AspNetCore.Components.Authorization

<AuthorizeView>
    <Authorized>
        Bienvenido, @context.User.Identity!.Name
        <a href="logout">Cerrar Sesión</a>
    </Authorized>
    <NotAuthorized>
        <a href="register">Registrarse</a>
        <a href="login">Iniciar Sesión</a>
    </NotAuthorized>
</AuthorizeView>
```

        - Dentro de "~/Layout/" :

            - "MainLayout.cs" :
```
...
@using Client.Pages
<div class="page">
    ...
    <main>
        <div class="top-row px-4">
            <LoginDisplay />
            ...
        </div>
        ...
    </main>
</div>
```

    - "Server" :

        - "~/JwtFeatures/JwtHandler.cs" :
```
...
namespace Server.JwtFeatures
{
    public class JwtHandler
    {
        ...
        // Método que recupera los "Claims" del usuario recibido como parámetro
        public List<Claim> GetClaims(Usuario user)
        {
            var claims = new List<Claim>
            {
                ...,
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!),
                ...
            };

            return claims;
        }
        ...
    }
}
```

    - "Client" :
      (Para depurar una aplicación de tipo "Aplicación Blazor para WebAssembly" es necesario realizarlo con "IIS Server")

        - "~/Helpers/ApiAuthenticationStateProvider.cs" :
```
...
namespace Client.Helpers
{
    // Hereda de la clase "AuthenticationStateProvider"
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        ...
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ...
            var claims = ParseClaimsFromJwt(savedToken);
            var expiry = Convert.ToInt64(claims.SingleOrDefault(x => x.Type == "exp")!.Value);
            var unixSeconds = DateTimeOffset.Now.ToUnixTimeSeconds();

            if (expiry < unixSeconds)
            {
                await _localStorage.RemoveItemAsync("authToken");
                return new AuthenticationState(new ClaimsPrincipal());
            }
            ...
        }

        public void MarkUserAsAuthenticated(string authToken)
        {
            var claims = ParseClaimsFromJwt(authToken);

            var authenticatedUser = new ClaimsPrincipal(
                new ClaimsIdentity(
                    claims,
                    "apiauth"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }
        ...
    }
}
```

        - "~/Services/AuthenticationService.cs" :
```
...
namespace Client.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        ...
        public async Task<AuthenticationResponseDto> Login(UserForAuthenticationDto loginModel)
        {
            ...
            ((ApiAuthenticationStateProvider)_stateProvider).MarkUserAsAuthenticated(loginResult.Token!);
            ...
        }
        ...
    }
}
```

    - "Shared" :

        - Dentro de "~/DataTransferObjects/" :

            - "UserForUpdateDto.cs" :
```
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Shared.DataTransferObjects
{
    public class UserForUpdateDto
    {
        [Required(ErrorMessage = "El nombre es requerido."),
            DataType(DataType.Text), Display(Name = "Nombre")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "El correo es requerido."),
            EmailAddress, Display(Name = "Correo")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "El usuario es requerido."),
            DataType(DataType.Text), Display(Name = "Usuario")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida."),
            DataType(DataType.Password), Display(Name = "Contraseña"),
            StringLength(100, ErrorMessage = "La {0} debe tener una longitud de entre {2} y {1} caracteres.", MinimumLength = 6)]
        public string? Password { get; set; }
    }

    public class PasswordForUpdateDto
    {
        [Required(ErrorMessage = "La contraseña actual es requerida."),
            DataType(DataType.Password), Display(Name = "Contraseña actual"),
            StringLength(100, ErrorMessage = "La {0} debe tener una longitud de entre {2} y {1} caracteres.", MinimumLength = 6)]
        public string? CurrentPassword { get; set; }

        [Required(ErrorMessage = "La nueva contraseña es requerida."),
            DataType(DataType.Password), Display(Name = "Nueva contraseña"),
            StringLength(100, ErrorMessage = "La {0} debe tener una longitud de entre {2} y {1} caracteres.", MinimumLength = 6)]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password), Display(Name = "Confirmar nueva contraseña"),
            Compare(nameof(NewPassword), ErrorMessage = "La nueva contraseña no coincide.")]
        public string? ConfirmNewPassword { get; set; }
    }

    public class ResponseDto<T>
    {
        public bool IsSuccessful { get; set; }
        public T? Element { get; set; }
        public string? Error { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
```

    - "Server" :

        - "MappingProfile.cs" :
```
...
namespace Server
{
    // Hereda de la clase "AutoMapper.Profile"
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ...
            CreateMap<UserForUpdateDto, Usuario>()
                .ForMember(u => u.NombreCompleto, opt => opt.MapFrom(x => x.FullName));
            CreateMap<Usuario, UserForUpdateDto>()
                .ForMember(x => x.FullName, opt => opt.MapFrom(u => u.NombreCompleto));
        }
    }
}
```

        - Creamos la carpeta "~/Services/" :

            - "IUsuariosService.cs" :
```
using Shared.DataTransferObjects;

namespace Server.Services
{
    public interface IUsuariosService
    {
        public Task<ResponseDto<UserForUpdateDto>> GetUsuario(string idUsuario);
        public Task<ResponseDto<UserForUpdateDto>> UpdateUsuario(UserForUpdateDto usuario, string idUsuario);
        public Task<ResponseDto<PasswordForUpdateDto>> UpdatePassword(PasswordForUpdateDto password, string idUsuario);
    }
}
```

            - "UsuariosService.cs" :
```
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Server.Models;
using Shared.DataTransferObjects;
using System.Net;

namespace Server.Services
{
    public class UsuariosService : IUsuariosService
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly IMapper _mapper;

        public UsuariosService(UserManager<Usuario> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ResponseDto<UserForUpdateDto>> GetUsuario(string idUsuario)
        {
            ResponseDto<UserForUpdateDto> response = new ResponseDto<UserForUpdateDto>();

            try
            {
                if (string.IsNullOrEmpty(idUsuario))
                {
                    response.Error = "El id de usuario es nulo o está vacío.";
                    response.StatusCode = HttpStatusCode.BadRequest;
                }
                else
                {
                    Usuario? user = await _userManager.FindByIdAsync(idUsuario);

                    if (user == null)
                    {
                        response.Error = "El usuario indicado no existe.";
                        response.StatusCode = HttpStatusCode.NotFound;
                    }
                    else
                    {
                        response.IsSuccessful = true;
                        response.Element = _mapper.Map<UserForUpdateDto>(user);
                        response.StatusCode = HttpStatusCode.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        public async Task<ResponseDto<UserForUpdateDto>> UpdateUsuario(UserForUpdateDto usuario, string idUsuario)
        {
            ResponseDto<UserForUpdateDto> response = new ResponseDto<UserForUpdateDto>();

            try
            {
                if (string.IsNullOrEmpty(idUsuario))
                {
                    response.Error = "El id de usuario es nulo o está vacío.";
                    response.StatusCode = HttpStatusCode.BadRequest;
                }
                else
                {
                    Usuario? user = await _userManager.FindByIdAsync(idUsuario);

                    if (user == null)
                    {
                        response.Error = "El usuario indicado no existe.";
                        response.StatusCode = HttpStatusCode.NotFound;
                    }
                    else
                    {
                        var isValid = await _userManager.CheckPasswordAsync(user, usuario.Password!);

                        if (!isValid)
                        {
                            response.Error = "La contraseña no es correcta.";
                            response.StatusCode = HttpStatusCode.Unauthorized;
                        }
                        else
                        {
                            _mapper.Map<UserForUpdateDto, Usuario>(usuario, user);

                            var result = await _userManager.UpdateAsync(user);

                            if (!result.Succeeded)
                            {
                                response.Error = result.Errors.First()!.Description;
                                response.StatusCode = HttpStatusCode.BadRequest;
                            }
                            else
                            {
                                response.IsSuccessful = true;
                                response.Element = _mapper.Map<UserForUpdateDto>(user);
                                response.StatusCode = HttpStatusCode.OK;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }

        public async Task<ResponseDto<PasswordForUpdateDto>> UpdatePassword(PasswordForUpdateDto password, string idUsuario)
        {
            ResponseDto<PasswordForUpdateDto> response = new ResponseDto<PasswordForUpdateDto>();

            try
            {
                if (string.IsNullOrEmpty(idUsuario))
                {
                    response.Error = "El id de usuario es nulo o está vacío.";
                    response.StatusCode = HttpStatusCode.BadRequest;
                }
                else
                {
                    Usuario? user = await _userManager.FindByIdAsync(idUsuario);

                    if (user == null)
                    {
                        response.Error = "El usuario indicado no existe.";
                        response.StatusCode = HttpStatusCode.NotFound;
                    }
                    else
                    {
                        var isValid = await _userManager.CheckPasswordAsync(user, password.CurrentPassword!);

                        if (!isValid)
                        {
                            response.Error = "La contraseña no es correcta.";
                            response.StatusCode = HttpStatusCode.Unauthorized;
                        }
                        else
                        {
                            await _userManager.ChangePasswordAsync(user, password.CurrentPassword!, password.NewPassword!);

                            response.IsSuccessful = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            return response;
        }
    }
}
```

        - "Program.cs" :
```
...
using Server.Services;
...
builder.Services.AddScoped<IUsuariosService, UsuariosService>();
...
```

        - Dentro de "~/Controllers/" :
            
            - "UsuariosController.cs" :
```
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using Shared.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;

namespace Server.Controllers
{
    [Authorize]
    [EnableCors(policyName: "MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuariosService _usuariosService;

        public UsuariosController(IUsuariosService usuariosService)
        {
            _usuariosService = usuariosService;
        }

        [HttpGet("GetUsuario")]
        public async Task<IActionResult> GetUsuario([FromHeader] string id)
        {
            var result = await _usuariosService.GetUsuario(id);

            return StatusCode(((int)result.StatusCode), result);
        }

        [HttpPut("UpdateUsuario")]
        public async Task<IActionResult> UpdateUsuario([FromBody] UserForUpdateDto model, [FromHeader] string id)
        {
            if (model == null || !ModelState.IsValid) return BadRequest();

            var result = await _usuariosService.UpdateUsuario(model, id);

            return StatusCode(((int)result.StatusCode), result);
        }

        [HttpPut("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] PasswordForUpdateDto model, [FromHeader] string id)
        {
            if (model == null || !ModelState.IsValid) return BadRequest();

            var result = await _usuariosService.UpdatePassword(model, id);

            return StatusCode(((int)result.StatusCode), result);
        }
    }
}
```

    - "Client" :

        - Creamos la carpeta "~/Services/" :

            - "UsuariosService.cs" :
```
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
        private readonly ILocalStorageService _localStorage;

        private readonly string apiUri = "https://localhost:7123/api/";

        public UsuariosService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
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
    }
}
```

        - "Program.cs" :
```
...
using Shared.Services;
...
builder.Services.AddScoped<IUsuariosService, UsuariosService>();
...
```

        - Dentro de "~/Pages/" :

            - "Perfil.razor" :
```
@page "/perfil"
@using Shared.DataTransferObjects
@using Shared.Services
@inject IUsuariosService UsuariosService
@inject NavigationManager NavigationManager
@using Microsoft.AspNetCore.Components.Authorization
@inject AuthenticationStateProvider StateProvider

<h3>Perfil</h3>

@if (showErrors)
{
    <div class="alert alert-danger" role="alert">
        <p>@error</p>
    </div>
}

<div class="card">
    <div class="card-body">
        <h5 class="card-title">Rellena el formulario.</h5>
        <EditForm Model="userModel" OnValidSubmit="UpdateUsuario">
            <DataAnnotationsValidator />
            <ValidationSummary />

            <div class="form-group">
                <label for="nombreCompleto">Nombre Completo</label>
                <InputText id="nombreCompleto" class="form-control" @bind-Value="userModel.FullName" />
                <ValidationMessage For="@(() => userModel.FullName)" />
            </div>
            <div class="form-group">
                <label for="email">Correo</label>
                <InputText id="email" class="form-control" @bind-Value="userModel.Email" />
                <ValidationMessage For="@(() => userModel.Email)" />
            </div>
            <div class="form-group">
                <label for="username">Usuario</label>
                <InputText id="userName" class="form-control" @bind-Value="userModel.UserName" />
                <ValidationMessage For="@(() => userModel.UserName)" />
            </div>
            <div class="form-group">
                <label for="password">Contraseña</label>
                <InputText id="password" type="password" class="form-control" @bind-Value="userModel.Password" />
                <ValidationMessage For="@(() => userModel.Password)" />
            </div>
            <hr />
            <div class="text-end">
                <button type="submit" class="btn btn-outline-primary">Enviar</button>
            </div>
        </EditForm>
    </div>
</div>

<div class="card">
    <div class="card-body">
        <h5 class="card-title">Rellena el formulario.</h5>
        <EditForm Model="passModel" OnValidSubmit="UpdatePassword">
            <DataAnnotationsValidator />
            <ValidationSummary />

            <div class="form-group">
                <label for="currentPassword">Contraseña Actual</label>
                <InputText id="currentPassword" type="password" class="form-control" @bind-Value="passModel.CurrentPassword" />
                <ValidationMessage For="@(() => passModel.CurrentPassword)" />
            </div>
            <div class="form-group">
                <label for="newPassword">Nueva Contraseña</label>
                <InputText id="newPassword" type="password" class="form-control" @bind-Value="passModel.NewPassword" />
                <ValidationMessage For="@(() => passModel.NewPassword)" />
            </div>
            <div class="form-group">
                <label for="confirmNewPassword">Confirmar Nueva Contraseña</label>
                <InputText id="confirmNewPassword" type="password" class="form-control" @bind-Value="passModel.ConfirmNewPassword" />
                <ValidationMessage For="@(() => passModel.ConfirmNewPassword)" />
            </div>
            <hr />
            <div class="text-end">
                <button type="submit" class="btn btn-outline-primary">Enviar</button>
            </div>
        </EditForm>
    </div>
</div>

@code {
    private UserForUpdateDto userModel = new UserForUpdateDto();
    private PasswordForUpdateDto passModel = new PasswordForUpdateDto();
    private bool showErrors;
    private string error = "";

    private string idUsuario = "";

    protected override async Task OnInitializedAsync()
    {
        idUsuario = (await StateProvider.GetAuthenticationStateAsync()).User.FindFirst("Id")!.Value;

        var result = await UsuariosService.GetUsuario(idUsuario);

        userModel = result.Element!;
    }

    private async Task UpdateUsuario()
    {
        showErrors = false;

        var result = await UsuariosService.UpdateUsuario(userModel, idUsuario);

        if (result.IsSuccessful)
        {
            userModel = result.Element!;
            return;
        }

        error = result.Error!;
        showErrors = true;
    }

    private async Task UpdatePassword()
    {
        showErrors = false;

        var result = await UsuariosService.UpdatePassword(passModel, idUsuario);

        if (result.IsSuccessful)
        {
            passModel = result.Element!;
            return;
        }

        error = result.Error!;
        showErrors = true;
    }
}
```