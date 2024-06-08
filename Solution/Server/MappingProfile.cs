using AutoMapper;
using ClassLibrary.DataTransferObjects;
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
