using ApiDomain.Enums;

namespace WebApp.Dtos.Movie
{
    public class MovieCreateDto
    {
        public string Title { get; set; } = null!;
        public string Director { get; set; } = null!;
        public string Genre { get; set; } = null!;
        public bool IsReleased { get; set; }
        public DateOnly ReleaseDate { get; set; }
    }
}
