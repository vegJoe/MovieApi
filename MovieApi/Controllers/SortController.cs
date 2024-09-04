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
    public class SortController : ControllerBase
    {
        private readonly MovieApiContext _db;
        private readonly IMapper _mapper;


        public SortController(MovieApiContext context, IMapper mapper)
        {
            _db = context;
            this._mapper = mapper;
        }

        [HttpGet("title")]
        public async Task<ActionResult<IEnumerable<MovieDetailsDto>>> GetMovieByTitle()
        {
            var allMovies = await _db.Movie.ToListAsync();

            var foundMovies = allMovies.OrderBy(m => m.Title).ToList();


            if (foundMovies == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<ICollection<MovieDetailsDto>>(foundMovies);

            return Ok(dto);
        }

        [HttpGet("date/{orderDirection?}")]
        public async Task<ActionResult<IEnumerable<MovieDetailsDto>>> GetMovieByDate(string orderDirection = "asc")
        {
            var allMovies = await _db.Movie.ToListAsync();
            var returnMovies = new List<Movie>();

            if(orderDirection.ToLower().Equals("desc"))
            {
                var foundMoviesDescending = allMovies.OrderByDescending(m => m.ReleaseDate).ToList();
                returnMovies = foundMoviesDescending;
            }
            else
            {
                var foundMovies = allMovies.OrderBy(m => m.ReleaseDate).ToList();
                returnMovies = foundMovies;
            }


            if (returnMovies == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<ICollection<MovieDetailsDto>>(returnMovies);

            return Ok(dto);
        }

        [HttpGet("rating/{orderDirection?}")]
        public async Task<ActionResult<IEnumerable<MovieDetailsDto>>> GetMovieByRating(string orderDirection = "asc")
        {
            var allMovies = await _db.Movie.ToListAsync();
            var returnMovies = new List<Movie>();

            if (orderDirection.ToLower().Equals("desc"))
            {
                var foundMoviesDescending = allMovies.OrderByDescending(m => m.Rating).ToList();
                returnMovies = foundMoviesDescending;
            }
            else
            {
                var foundMovies = allMovies.OrderBy(m => m.Rating).ToList();
                returnMovies = foundMovies;
            }


            if (returnMovies == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<ICollection<MovieDetailsDto>>(returnMovies);

            return Ok(dto);
        }

        private bool MovieExists(int id)
        {
            return _db.Movie.Any(e => e.Id == id);
        }
    }
}
