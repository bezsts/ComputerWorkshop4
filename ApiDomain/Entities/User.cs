namespace ApiDomain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<Movie> WatchedMovies { get; set; } = [];
    }
}
