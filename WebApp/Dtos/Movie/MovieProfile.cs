using AutoMapper;
using MovieEntity = ApiDomain.Entities.Movie;

namespace WebApp.Dtos.Movie
{
    public class MovieProfile : Profile
    {
        public MovieProfile() 
        {
            CreateMap<MovieCreateDto, MovieEntity>()
                .ForMember(dest => dest.UsersWhoWatched, opt => opt.Ignore());

            CreateMap<MovieEntity, MovieOutputDto>()
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.ToString()))
                .ForMember(dest => dest.ViewCount, opt => opt.MapFrom(src => src.UsersWhoWatched.Count()));
        }
    }
}
