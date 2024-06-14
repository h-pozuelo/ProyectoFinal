using AutoMapper;
using Shared.DataTransferObjects;
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

            CreateMap<UserForUpdateDto, Usuario>()
                .ForMember(u => u.NombreCompleto, opt => opt.MapFrom(x => x.FullName));
            CreateMap<Usuario, UserForUpdateDto>()
                .ForMember(x => x.FullName, opt => opt.MapFrom(u => u.NombreCompleto));

            CreateMap<AlojamientoDto, Alojamiento>()
                .ForMember(a => a.Propietario, opt => opt.Ignore())
                .ForMember(a => a.Alquileres, opt => opt.Ignore());
            CreateMap<Alojamiento, AlojamientoDto>();

            CreateMap<AlquilerDto, Alquiler>()
                .ForMember(al => al.Alojamiento, opt => opt.Ignore())
                .ForMember(al => al.Inquilino, opt => opt.Ignore());
            CreateMap<Alquiler, AlquilerDto>();
        }
    }
}
