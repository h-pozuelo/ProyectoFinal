using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
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
