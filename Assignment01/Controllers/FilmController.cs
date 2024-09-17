using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment01.Context;
using Assignment01.DTO;
using Assignment01.DTO.Request;
using Assignment01.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Assignment01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmController : ControllerBase
    {
        private readonly MyDbContext _context;

        public FilmController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Film
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FilmDTO>>> GetFilms()
        {
          if (_context.Films == null)
          {
              return NotFound();
          }
          return _context.Films.Select(f => new FilmDTO
          {
              FilmID = f.FilmID,
              Genre = _context.Genres.FirstOrDefault(g => g.GenreID == f.GenreID).Name,
              Title = f.Title,
              Year = f.Year,
              CountryCode = f.CountryCode,
              FilmUrl = f.FilmUrl
          }).ToList();
        }

        // GET: api/Film/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FilmDTO>> GetFilm(int id)
        {
          if (_context.Films == null)
          {
              return NotFound();
          }
            var film = await _context.Films.FindAsync(id);

            if (film == null)
            {
                return NotFound();
            }

            return new FilmDTO
            {
                FilmID = film.FilmID,
                Genre = _context.Genres.FirstOrDefault(g => g.GenreID == film.GenreID).Name,
                Title = film.Title,
                Year = film.Year,
                CountryCode = film.CountryCode,
                FilmUrl = film.FilmUrl
            };
        }

        // PUT: api/Film/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFilm(int id, FilmRequestDTO film)
        {
            if (id != film.FilmID)
            {
                return BadRequest();
            }

            var filmEntity = new Film
            {
                FilmID = film.FilmID,
                GenreID = film.GenreID,
                Title = film.Title,
                Year = film.Year,
                CountryCode = film.CountryCode,
                FilmUrl = film.FilmUrl
            };
            
            _context.Entry(filmEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilmExists(id))
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

        // POST: api/Film
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FilmDTO>> PostFilm(FilmRequestDTO film)
        {
          if (_context.Films == null)
          {
              return Problem("Entity set 'MyDbContext.Films'  is null.");
          }

          var filmEntity = new Film
          {
                FilmID = film.FilmID,
                GenreID = film.GenreID,
                Title = film.Title,
                Year = film.Year,
                CountryCode = film.CountryCode,
                FilmUrl = film.FilmUrl
          };
          
            _context.Films.Add(filmEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFilm", new { id = film.FilmID }, film);
        }

        // DELETE: api/Film/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFilm(int id)
        {
            if (_context.Films == null)
            {
                return NotFound();
            }
            var film = await _context.Films.FindAsync(id);
            if (film == null)
            {
                return NotFound();
            }

            _context.Films.Remove(film);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FilmExists(int id)
        {
            return (_context.Films?.Any(e => e.FilmID == id)).GetValueOrDefault();
        }
    }
}
