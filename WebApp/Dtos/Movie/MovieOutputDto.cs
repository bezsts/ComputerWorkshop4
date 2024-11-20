namespace WebApp.Dtos.Movie
{
    public class MovieOutputDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Director { get; set; } = null!;
        public string Genre { get; set; } = null!;
        public bool IsReleased { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public int ViewCount { get; set; }
    }
}
