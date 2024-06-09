using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObjects;
using Server.JwtFeatures;
using Server.Models;
using System.IdentityModel.Tokens.Jwt;

namespace Server.Controllers
{
    [EnableCors(policyName: "MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<Usuario> _userManager;
        // Necesario para mapear
        private readonly IMapper _mapper;

        // Necesario para autenticar usuarios en el servidor
        private readonly SignInManager<Usuario> _signInManager;
        // Necesario para recuperar las opciones de Jwt configuradas en el servidor
        private readonly JwtHandler _jwtHandler;

        public AccountsController(UserManager<Usuario> userManager, IMapper mapper, SignInManager<Usuario> signInManager, JwtHandler jwtHandler)
        {
            _userManager = userManager;
            _mapper = mapper;
            // Gestiona credenciales del tipo "IdentityUser<Usuario>"
            _signInManager = signInManager;
            _jwtHandler = jwtHandler;
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
