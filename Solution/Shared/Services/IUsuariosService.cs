using Shared.DataTransferObjects;

namespace Shared.Services
{
    public interface IUsuariosService
    {
        public Task<ResponseDto<UserForUpdateDto>> GetUsuario(string idUsuario);
        public Task<ResponseDto<UserForUpdateDto>> UpdateUsuario(UserForUpdateDto usuario, string idUsuario);
        public Task<ResponseDto<PasswordForUpdateDto>> UpdatePassword(PasswordForUpdateDto password, string idUsuario);
    }
}
