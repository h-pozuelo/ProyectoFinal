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
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!),
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
