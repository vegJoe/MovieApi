namespace MovieApi.Models.Dtos
{
    public record ActorsDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int DateOfBirth { get; set; }
    }
}
