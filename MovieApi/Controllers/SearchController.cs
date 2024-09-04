using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Models.Dtos;
using MovieApi.Models.Entities;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly MovieApiContext _db;
        private readonly IMapper _mapper;

        public SearchController(MovieApiContext context, IMapper mapper)
        {
            _db = context;
            this._mapper = mapper;
        }

        [HttpGet("title/{title}")]
        public async Task<ActionResult<IEnumerable<MovieDetailsDto>>> GetMovieByTitle(string title)
        {
            //var allMovies = await _db.Movie.ToListAsync();  // Hämta alla filmer
            //var foundMovies = allMovies
            //    .Where(x => x.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
            //    .ToList();

            var foundMovies = await _db.Movie
                .Where(x => x.Title.ToLower().Contains(title.ToLower()))
                .ToListAsync();


            if (foundMovies == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<ICollection<MovieDetailsDto>>(foundMovies);

            return Ok(dto);
        }

        [HttpGet("genre/{genreName}")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovieByGenre(string genreName)
        {
            var movies = await _db.Movie
                .Where(m => m.Genres.Any(g => g.Name.ToLower().Contains(genreName.ToLower()))).Include(m => m.Genres)
                .ToListAsync();


            if (movies == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<IEnumerable<MovieDetailsDto>>(movies);

            return Ok(dto);
        }
    }
}