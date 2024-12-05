using WebApp.Dtos.Movie;

namespace WebApp.Dtos.User
{
    public class UserWatchedMoviesListOutputDto
    {
        public int Id { get; set; }
        public List<MovieOutputDto> WatchedMovies { get; set; } = [];
    }
}
