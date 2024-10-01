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
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Assignment01.Controllers
{
    [Route("odata/Film")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class FilmController : ODataController
    {
        private readonly MyDbContext _context;

        public FilmController(MyDbContext context)
        {
            _context = context;
        }
        
        [EnableQuery]
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_context.Films);
        }

        // GET: odata/Film({id})
        [HttpGet("{key}")]
        [EnableQuery]
        public ActionResult Get([FromRoute]int key)
        {
            var film = _context.Films.Where(s => s.FilmID == key);
            if (!film.Any())
            {
                return NotFound();
            }
            return Ok(SingleResult.Create(film));
        }
        

        
        // [HttpGet("search/{title}")]
        // public async Task<ActionResult<List<FilmResponseDTO>>> GetFilmByTitle(string title)
        // {
        //     if (_context.Films == null)
        //     {
        //         return NotFound();
        //     }
        //     var filmList = await _context.Films.Select(f => new FilmResponseDTO
        //     {
        //         FilmID = f.FilmID,
        //         Genre = _context.Genres.FirstOrDefault(g => g.GenreID == f.GenreID).Name,
        //         Title = f.Title,
        //         Year = f.Year,
        //         CountryName = _context.Countries.FirstOrDefault(c => c.CountryCode == f.CountryCode).CountryName,
        //         FilmUrl = f.FilmUrl
        //     }).Where(f => f.Title.Contains(title)).ToListAsync();
        //
        //     return filmList;
        // }

        // PUT: api/Film/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // POST: odata/Show
        [HttpPost]
        public IActionResult Post([FromBody] FilmDTO film)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newFilmEntity = new Film
            {
                GenreID = film.GenreID,
                Title = film.Title,
                Year = film.Year,
                CountryCode = film.CountryCode,
                FilmUrl = film.FilmUrl
            };
            
            _context.Films.Add(newFilmEntity);
            _context.SaveChanges();

            return Created(film);
        }

        // PUT: odata/Show(5)
        [HttpPut("{key}")]
        public IActionResult Put([FromRoute] int key, [FromBody] FilmDTO updateFilm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingFilm = _context.Films.Find(key);
            if (existingFilm == null)
            {
                return NotFound();
            }

            // Cập nhật Film hiện có
            existingFilm.GenreID = updateFilm.GenreID;
            existingFilm.Title = updateFilm.Title;
            existingFilm.Year = updateFilm.Year;
            existingFilm.CountryCode = updateFilm.CountryCode;
            existingFilm.FilmUrl = updateFilm.FilmUrl;

            _context.Entry(existingFilm).State = EntityState.Modified;
            _context.SaveChanges();

            return Updated(existingFilm);
        }

        // DELETE: odata/Show(5)
        [HttpDelete("{key}")]
        public IActionResult Delete([FromRoute] int key)
        {
            var film = _context.Films.Find(key);
            if (film == null)
            {
                return NotFound();
            }

            _context.Films.Remove(film);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
