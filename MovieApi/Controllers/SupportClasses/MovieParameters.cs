namespace MovieApi.Controllers.SupportClasses
{
    public class MovieParameters
    {
        public string ActorName { get; set; }
        public int? ReleaseDate { get; set; }
        public string? DirectorName { get; set; }
        public string? GenreName { get; set; }
        public string? SortBy { get; set; }
    }
}
