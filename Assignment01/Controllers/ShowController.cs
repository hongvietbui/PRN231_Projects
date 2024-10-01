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
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Assignment01.Controllers
{
    [Route("odata/Show")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ShowController : ODataController
    {
        private readonly MyDbContext _context;

        public ShowController(MyDbContext context)
        {
            _context = context;
        }
        //
        // // GET: api/Show
        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<ShowDTO>>> GetShows()
        // {
        //   if (_context.Shows == null)
        //   {
        //       return NotFound();
        //   }
        //     return await _context.Shows.Select(s => new ShowDTO
        //     {
        //         ShowID = s.ShowID,
        //         FilmID = s.FilmID,
        //         ShowDate = s.ShowDate,
        //         Price = s.Price,
        //         Status = s.Status,
        //         Slot = s.Slot,
        //         RoomID = s.RoomID
        //     }).ToListAsync();
        // }
        //
        // // GET: api/Show/5
        // [HttpGet("{id}")]
        // public async Task<ActionResult<ShowDTO>> GetShow(int id)
        // {
        //   if (_context.Shows == null)
        //   {
        //       return NotFound();
        //   }
        //     var show = await _context.Shows.FindAsync(id);
        //
        //     if (show == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     return new ShowDTO
        //     {
        //         ShowID = show.ShowID,
        //         FilmID = show.FilmID,
        //         ShowDate = show.ShowDate,
        //         Price = show.Price,
        //         Status = show.Status,
        //         Slot = show.Slot,
        //         RoomID = show.RoomID
        //     };
        // }
        //
        // // GET: api/Show/search/{title}
        // [HttpGet("search/{showDate}/{selectedRoomId}")]
        // public async Task<ActionResult<List<ShowDTO>>> GetShow(DateTime showDate, int selectedRoomId)
        // {
        //     var shows = await _context.Shows.Where(s => s.ShowDate == showDate && s.RoomID == selectedRoomId).ToListAsync();
        //     
        //     if (shows == null)
        //     {
        //         return NotFound();
        //     }
        //     
        //     return shows.Select(s => new ShowDTO
        //     {
        //         ShowID = s.ShowID,
        //         FilmID = s.FilmID,
        //         ShowDate = s.ShowDate,
        //         Price = s.Price,
        //         Status = s.Status,
        //         Slot = s.Slot,
        //         RoomID = s.RoomID
        //     }).ToList();
        // }
        //
        //
        // // PUT: api/Show/5
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutShow(int id, ShowDTO showDTO)
        // {
        //     if (id != showDTO.ShowID)
        //     {
        //         return BadRequest();
        //     }
        //
        //     var showEnity = new Show
        //     {
        //         ShowID = showDTO.ShowID,
        //         FilmID = showDTO.FilmID,
        //         ShowDate = showDTO.ShowDate,
        //         Price = showDTO.Price,
        //         Status = showDTO.Status,
        //         Slot = showDTO.Slot,
        //         RoomID = showDTO.RoomID
        //     };
        //
        //     _context.Entry(showEnity).State = EntityState.Modified;
        //
        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!ShowExists(id))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }
        //
        //     return NoContent();
        // }
        //
        // // POST: api/Show
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPost]
        // public async Task<ActionResult<ShowDTO>> PostShow(ShowDTO show)
        // {
        //   if (_context.Shows == null)
        //   {
        //       return Problem("Entity set 'MyDbContext.Shows'  is null.");
        //   }
        //   
        //   var newShowEntity = new Show
        //   {
        //         ShowID = show.ShowID,
        //         FilmID = show.FilmID,
        //         ShowDate = show.ShowDate,
        //         Price = show.Price,
        //         Status = show.Status,
        //         Slot = show.Slot,
        //         RoomID = show.RoomID
        //     };
        //     
        //         _context.Shows.Add(newShowEntity);
        //         await _context.SaveChangesAsync();
        //
        //         return CreatedAtAction("GetShow", new { id = show.ShowID }, show);
        //   }
        //
        // // DELETE: api/Show/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteShow(int id)
        // {
        //     if (_context.Shows == null)
        //     {
        //         return NotFound();
        //     }
        //     var show = await _context.Shows.FindAsync(id);
        //     if (show == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     _context.Shows.Remove(show);
        //     await _context.SaveChangesAsync();
        //
        //     return NoContent();
        // }
        //
        // [HttpGet("getslot/{date}")]
        // public async Task<ActionResult<List<int>>> GetShowSlotByDate(DateTime date)
        // {
        //     // Fetch the booked slots for the given date
        //     var bookedSlots = await _context.Shows
        //         .Where(b => b.ShowDate == date.Date)
        //         .Select(b => b.Slot)
        //         .Distinct()
        //         .ToListAsync();
        //     var allSlots = Enumerable.Range(1, 9).ToList();
        //
        //     // Exclude booked slots from the list of available slots
        //     var availableSlots = allSlots.Except(bookedSlots).ToList();
        //
        //
        //     return availableSlots;
        // }
        //
        // private bool ShowExists(int id)
        // {
        //     return (_context.Shows?.Any(e => e.ShowID == id)).GetValueOrDefault();
        // }
        // GET: odata/Show
        // GET: odata/Show
        [EnableQuery] // Cho phép query OData
        [HttpGet] // Định nghĩa phương thức GET cho toàn bộ Show
        public IActionResult Get()
        {
            return Ok(_context.Shows); // Trả về tất cả các Show
        }

        // GET: odata/Show({key})
        [EnableQuery]
        [HttpGet("{key}")] // Định nghĩa phương thức GET cho một Show cụ thể theo key
        public IActionResult Get([FromRoute] int key)
        {
            var show = _context.Shows.Where(s => s.ShowID == key);
            if (!show.Any())
            {
                return NotFound();
            }
            return Ok(SingleResult.Create(show));
        }

        // POST: odata/Show
        [HttpPost]
        public IActionResult Post([FromBody] ShowDTO show)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
            _context.SaveChanges();

            return Created(show);
        }

        // PUT: odata/Show(5)
        [HttpPut("{key}")]
        public IActionResult Put([FromRoute] int key, [FromBody] ShowDTO updateShow)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingShow = _context.Shows.Find(key);
            if (existingShow == null)
            {
                return NotFound();
            }

            // Cập nhật show hiện có
            existingShow.RoomID = updateShow.RoomID;
            existingShow.FilmID = updateShow.FilmID;
            existingShow.ShowDate = updateShow.ShowDate;
            existingShow.Price = updateShow.Price;
            existingShow.Status = updateShow.Status;
            existingShow.Slot = updateShow.Slot;

            _context.Entry(existingShow).State = EntityState.Modified;
            _context.SaveChanges();

            return Updated(existingShow);
        }

        // DELETE: odata/Show(5)
        [HttpDelete("{key}")]
        public IActionResult Delete([FromRoute] int key)
        {
            var show = _context.Shows.Find(key);
            if (show == null)
            {
                return NotFound();
            }

            _context.Shows.Remove(show);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
