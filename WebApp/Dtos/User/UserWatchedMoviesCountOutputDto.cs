namespace WebApp.Dtos.User
{
    public class UserWatchedMoviesCountOutputDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int WatchedMoviesCount { get; set; }
    }
}
