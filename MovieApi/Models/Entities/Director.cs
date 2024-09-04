namespace MovieApi.Models.Entities
{
    public class Director
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ContactInformationId { get; set; }

        public ContactInformation ContactInformation { get; set; }
    }
}
