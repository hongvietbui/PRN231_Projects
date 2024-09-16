using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Assignment01.Context;
using Assignment01.Entities;

namespace Assignment01_RazorPages.Pages.Shows
{
    public class CreateModel : PageModel
    {
        private readonly Assignment01.Context.MyDbContext _context;

        public CreateModel(Assignment01.Context.MyDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["FilmID"] = new SelectList(_context.Films, "FilmID", "CountryCode");
        ViewData["RoomID"] = new SelectList(_context.Rooms, "RoomID", "RoomID");
            return Page();
        }

        [BindProperty]
        public Show Show { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Shows == null || Show == null)
            {
                return Page();
            }

            _context.Shows.Add(Show);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
