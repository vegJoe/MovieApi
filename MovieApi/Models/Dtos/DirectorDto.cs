using MovieApi.Models.Entities;

namespace MovieApi.Models.Dtos
{
    public record DirectorDto
    {
        public string Name { get; set; }

        public ContactInformation ContactInformation { get; set; }
    }
}
