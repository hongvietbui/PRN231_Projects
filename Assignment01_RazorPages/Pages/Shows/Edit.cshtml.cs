using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assignment01.Context;
using Assignment01.Entities;

namespace Assignment01_RazorPages.Pages.Shows
{
    public class EditModel : PageModel
    {
        private readonly Assignment01.Context.MyDbContext _context;

        public EditModel(Assignment01.Context.MyDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Show Show { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Shows == null)
            {
                return NotFound();
            }

            var show =  await _context.Shows.FirstOrDefaultAsync(m => m.ShowID == id);
            if (show == null)
            {
                return NotFound();
            }
            Show = show;
           ViewData["FilmID"] = new SelectList(_context.Films, "FilmID", "CountryCode");
           ViewData["RoomID"] = new SelectList(_context.Rooms, "RoomID", "RoomID");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Show).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShowExists(Show.ShowID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ShowExists(int id)
        {
          return (_context.Shows?.Any(e => e.ShowID == id)).GetValueOrDefault();
        }
    }
}
