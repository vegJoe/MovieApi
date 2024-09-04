namespace MovieApi.Models.Entities
{
    public class Genre
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Movie> MovieGenres { get; set; }
    }
}
