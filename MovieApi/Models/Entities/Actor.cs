namespace MovieApi.Models.Entities
{
    public class Actor
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int DateOfBirth { get; set; }

        public ICollection<Movie> StaredIn { get; set; }
    }
}
