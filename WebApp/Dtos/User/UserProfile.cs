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
            CreateMap<UserEntity, UserWatchedMoviesCountOutputDto>()
                .ForMember(dest => dest.WatchedMoviesCount, opt => opt.MapFrom(src => src.WatchedMovies.Count));
            CreateMap<UserEntity, UserWatchedMoviesListOutputDto>();
            CreateMap<UserEntity, UserOutputDto>();
        }
    }
}
