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
            CreateMap<Movie, MovieDetailsDto>();
            CreateMap<Actor, ActorsDto>();
            CreateMap<Genre, GenreDto>();
            CreateMap<Director, DirectorDto>();
            CreateMap<ContactInformation, ContactInformationDto>();
        }
    }
}
