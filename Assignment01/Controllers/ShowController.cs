using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment01.Context;
using Assignment01.DTO;
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


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShowDTO>>> GetShows()
        {
            if (_context.Shows == null)
            {
                return NotFound();
            }
            return await _context.Shows.Select(s => new ShowDTO
            {
                ShowID = s.ShowID,
                FilmID = s.FilmID,
                ShowDate = s.ShowDate,
                Price = s.Price,
                Status = s.Status,
                Slot = s.Slot,
                RoomID = s.RoomID
            }).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShowDTO>> GetShow(int id)
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

            return new ShowDTO
            {
                ShowID = show.ShowID,
                FilmID = show.FilmID,
                ShowDate = show.ShowDate,
                Price = show.Price,
                Status = show.Status,
                Slot = show.Slot,
                RoomID = show.RoomID
            };
        }

        [HttpGet("getslot/{date}")]
        public async Task<ActionResult<List<int>>> GetShowSlotByDate(DateTime date)
        {
            // Fetch the booked slots for the given date
            var bookedSlots = await _context.Shows
                .Where(b => b.ShowDate == date.Date)
                .Select(b => b.Slot)
                .Distinct()
                .ToListAsync();
            var allSlots = Enumerable.Range(1, 9).ToList();

            // Exclude booked slots from the list of available slots
            var availableSlots = allSlots.Except(bookedSlots).ToList();
            

            return availableSlots;
        }

        [HttpGet("search/{showDate}/{selectedRoomId}")]
        public async Task<ActionResult<List<ShowDTO>>> GetShow(DateTime showDate, int selectedRoomId)
        {
            var shows = await _context.Shows.Where(s => s.ShowDate == showDate && s.RoomID == selectedRoomId).ToListAsync();

            if (shows == null)
            {
                return NotFound();
            }

            return shows.Select(s => new ShowDTO
            {
                ShowID = s.ShowID,
                FilmID = s.FilmID,
                ShowDate = s.ShowDate,
                Price = s.Price,
                Status = s.Status,
                Slot = s.Slot,
                RoomID = s.RoomID
            }).ToList();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutShow(int id, ShowDTO showDTO)
        {
            if (id != showDTO.ShowID)
            {
                return BadRequest();
            }

            var showEnity = new Show
            {
                ShowID = showDTO.ShowID,
                FilmID = showDTO.FilmID,
                ShowDate = showDTO.ShowDate,
                Price = showDTO.Price,
                Status = showDTO.Status,
                Slot = showDTO.Slot,
                RoomID = showDTO.RoomID
            };

            _context.Entry(showEnity).State = EntityState.Modified;

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

        [HttpPost]
        public async Task<ActionResult<ShowDTO>> PostShow(ShowDTO show)
        {
            if (_context.Shows == null)
            {
                return Problem("Entity set 'MyDbContext.Shows'  is null.");
            }

            var newShowEntity = new Show
            {
                ShowID = show.ShowID,
                FilmID = show.FilmID,
                ShowDate = show.ShowDate,
                Price = show.Price,
                Status = show.Status,
                Slot = show.Slot,
                RoomID = show.RoomID
            };

            _context.Shows.Add(newShowEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShow", new { id = show.ShowID }, show);
        }


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
