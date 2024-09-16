using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment01.Context;
using Assignment01.Entities;

namespace Assignment01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowController : ControllerBase
    {
        private readonly MyDbContext _context;

        public ShowController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Show
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Show>>> GetShows()
        {
          if (_context.Shows == null)
          {
              return NotFound();
          }
            return await _context.Shows.ToListAsync();
        }

        // GET: api/Show/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Show>> GetShow(int id)
        {
          if (_context.Shows == null)
          {
              return NotFound();
          }
            var show = await _context.Shows.FindAsync(id);

            if (show == null)
            {
                return NotFound();
            }

            return show;
        }

        // PUT: api/Show/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShow(int id, Show show)
        {
            if (id != show.ShowID)
            {
                return BadRequest();
            }

            _context.Entry(show).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShowExists(id))
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

        // POST: api/Show
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Show>> PostShow(Show show)
        {
          if (_context.Shows == null)
          {
              return Problem("Entity set 'MyDbContext.Shows'  is null.");
          }
            _context.Shows.Add(show);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShow", new { id = show.ShowID }, show);
        }

        // DELETE: api/Show/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShow(int id)
        {
            if (_context.Shows == null)
            {
                return NotFound();
            }
            var show = await _context.Shows.FindAsync(id);
            if (show == null)
            {
                return NotFound();
            }

            _context.Shows.Remove(show);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShowExists(int id)
        {
            return (_context.Shows?.Any(e => e.ShowID == id)).GetValueOrDefault();
        }
    }
}
