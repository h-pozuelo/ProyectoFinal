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
