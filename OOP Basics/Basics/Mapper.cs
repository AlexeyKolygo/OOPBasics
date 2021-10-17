using AutoMapper;
using Basics.Models;
using Entity.DB.Models;

namespace Basics
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<UserModel, User>();
            CreateMap<Account, UserAccount>();
        }
    }
}
