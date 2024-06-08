using System.ComponentModel.DataAnnotations;

namespace ClassLibrary.DataTransferObjects
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
