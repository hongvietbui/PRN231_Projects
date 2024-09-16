using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment01.Context;
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
        public async Task<ActionResult<IEnumerable<Film>>> GetFilms()
        {
          if (_context.Films == null)
          {
              return NotFound();
          }
            return await _context.Films.ToListAsync();
        }

        // GET: api/Film/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Film>> GetFilm(int id)
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

            return film;
        }

        // PUT: api/Film/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFilm(int id, Film film)
        {
            if (id != film.FilmID)
            {
                return BadRequest();
            }

            _context.Entry(film).State = EntityState.Modified;

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
        public async Task<ActionResult<FilmController>> PostFilm(Film film)
        {
          if (_context.Films == null)
          {
              return Problem("Entity set 'MyDbContext.Films'  is null.");
          }
            _context.Films.Add(film);
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
