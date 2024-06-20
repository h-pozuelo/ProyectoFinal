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
