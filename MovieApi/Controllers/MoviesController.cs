using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.Models.Dtos;
using MovieApi.Models.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieApiContext _db;
        private readonly IMapper _mapper;

        public MoviesController(MovieApiContext context, IMapper mapper)
        {
            _db = context;
            this._mapper = mapper;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovie()
        {
            var movies = await _db.Movie
                .Include(m => m.Genres)
                .Include(m => m.Director)
                    .ThenInclude(d => d.ContactInformation)
                .Include(m => m.Actors)
                .ToListAsync();

            var movieDto = _mapper.Map<ICollection<MovieDetailsDto>>(movies);

            return Ok(movieDto);
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _db.Movie
                .Include(m => m.Director).ThenInclude(d => d.ContactInformation)
                .Include(m => m.Genres).Where(g => g.Id == id)
                .Include(m => m.Actors).Where(a => a.Id == id)
                .FirstOrDefaultAsync(m => m.Id == id);

            var movieDto = _mapper.Map<MovieDetailsDto>(movie);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movieDto);
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            _db.Entry(movie).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> PatchMovie(int id, JsonPatchDocument<Movie> patchDoc)
        {
            var movieById = await _db.Movie
                .Include(m => m.Genres)
                .Include(m => m.Actors)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movieById == null)
            {
                return NotFound();
            }

            foreach (var operation in patchDoc.Operations)
            {
                if (operation.path == "/actors/-" && operation.op == "add")
                {
                    var actorId = Convert.ToInt32(operation.value.ToString());

                    var actor = await _db.Actor.FindAsync(actorId);

                    if (actor == null)
                    {
                        return NotFound($"Actor with Id {actorId} not found.");
                    }

                    movieById.Actors.Add(actor);

                    return Ok();
                }
                else if (operation.path == "/genres/-" && operation.op == "add")
                {
                    var actorId = Convert.ToInt32(operation.value.ToString());

                    var actor = await _db.Actor.FindAsync(actorId);

                    if (actor == null)
                    {
                        return NotFound($"Actor with Id {actorId} not found.");
                    }

                    movieById.Actors.Add(actor);

                    return Ok();
                }
            }

            await _db.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(NewMovieDto movie)
        {
            var newMovie = _mapper.Map<Movie>(movie);
            if(newMovie != null)
            {
                _db.Movie.Add(newMovie);
                await _db.SaveChangesAsync();
                return CreatedAtAction("GetMovie", new { id = newMovie.Id }, movie);
            }
            return BadRequest("Missing fields in movie object");
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _db.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _db.Movie.Remove(movie);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _db.Movie.Any(e => e.Id == id);
        }
    }
}
