using ApiDomain.Enums;

namespace ApiDomain.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Director { get; set; } = null!;
        public Genre Genre { get; set; }
        public bool IsReleased { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public List<User> UsersWhoWatched { get; set; } = [];
    }
}
