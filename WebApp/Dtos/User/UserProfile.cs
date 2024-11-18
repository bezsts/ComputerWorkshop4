using AutoMapper;
using UserEntity = ApiDomain.Entities.User;

namespace WebApp.Dtos.User
{
    public class UserProfile : Profile
    {
       public UserProfile() 
       {
            CreateMap<UserCreateDto, UserEntity>()
                .ForMember(dest => dest.WatchedMovies, opt => opt.Ignore());
            CreateMap<UserEntity, UserOutputDto>()
                .ForMember(dest => dest.WatchedMoviesCount, opt => opt.MapFrom(src => src.WatchedMovies.Count));
        }
    }
}
