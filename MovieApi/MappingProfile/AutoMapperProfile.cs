using AutoMapper;
using Microsoft.Extensions.Options;
using MovieApi.Models.Dtos;
using MovieApi.Models.Entities;

namespace MovieApi.MappingProfile
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<Movie, MovieDetailsDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Director.ContactInformation.Email))
            .ForMember(dest => dest.Phonenumber, opt => opt.MapFrom(src => src.Director.ContactInformation.Phonenumber)).ReverseMap();

            CreateMap<Actor, ActorsDto>().ReverseMap();
            CreateMap<Genre, GenreDto>().ReverseMap();
            CreateMap<Director, DirectorDto>().ReverseMap();
            CreateMap<ContactInformation, ContactInformationDto>().ReverseMap();
        }
    }
}
