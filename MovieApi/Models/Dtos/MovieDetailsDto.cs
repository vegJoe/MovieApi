namespace MovieApi.Models.Dtos
{
    public record MovieActorsDto(string Name, int DateOfBirth);
    public record MovieGenresDto(string Name);
    public class MovieDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Rating { get; set; }
        public int ReleaseDate { get; set; }
        public string Description { get; set; }
        public string DirectorName { get; set; }
        public string Email { get; set; }
        public int Phonenumber { get; set; }
        public IEnumerable<GenreDto> Genres { get; set; }

        public IEnumerable<ActorsDto> Actors { get; set; }
    }
}
