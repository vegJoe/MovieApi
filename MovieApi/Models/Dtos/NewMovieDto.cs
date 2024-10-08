﻿using MovieApi.Validations;
using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.Dtos
{
    public class NewMovieDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [TitleIsUnique]
        public string Title { get; set; }
        [Range(0, 10)]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Release date is required in format yyyyMMdd.")]
        [DateLimit]
        public int ReleaseDate { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required()]
        public int DirectorId { get; set; }
    }
}
