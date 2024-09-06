using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.Dtos
{
    public class ContactInformationDto
    {
        [EmailAddress]
        public string Email { get; set; }

        public int Phonenumber { get; set; }
    }
}
