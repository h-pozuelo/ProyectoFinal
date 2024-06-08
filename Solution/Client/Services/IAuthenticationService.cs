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
