using ApiDomain.Enums;

namespace WebApp.Dtos.Movie
{
    public class MovieCreateDto
    {
        public string Title { get; set; } = null!;
        public string Director { get; set; } = null!;
        public Genre Genre { get; set; }
        public bool IsReleased { get; set; }
        public DateOnly ReleaseDate { get; set; }
    }
}
