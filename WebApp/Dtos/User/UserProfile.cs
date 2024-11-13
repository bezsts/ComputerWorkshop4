using AutoMapper;
using UserEntity = ApiDomain.Entities.User;

namespace WebApp.Dtos.User
{
    public class UserProfile : Profile
    {
       public UserProfile() 
       {
            CreateMap<UserEntity, UserOutputDto>();
       }
    }
}
