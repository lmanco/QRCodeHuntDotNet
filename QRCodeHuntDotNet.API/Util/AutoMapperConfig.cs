using AutoMapper;
using QRCodeHuntDotNet.API.DAL.Models;

namespace QRCodeHuntDotNet.API.Util
{
    public class AutoMapperConfig
    {
        public static MapperConfiguration GetConfig()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<UserRequestDTO, User>();
                config.CreateMap<User, UserResponseDTO>();
            });
        }
    }
}
