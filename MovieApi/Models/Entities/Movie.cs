using System.IO;

namespace MovieApi.Models.Entities
{
    public class Movie
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int Rating { get; set; }

        public int ReleaseDate { get; set; }

        public string Description { get; set; }

        public int DirectorId { get; set; }
        public Director Director { get; set; }

        public ICollection<Actor> Actors { get; set; }

        public ICollection<Genre> Genres { get; set; }
    }
}
