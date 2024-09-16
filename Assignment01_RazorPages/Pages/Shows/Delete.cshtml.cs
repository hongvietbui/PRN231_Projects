using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Assignment01.Context;
using Assignment01.Entities;

namespace Assignment01_RazorPages.Pages.Shows
{
    public class DeleteModel : PageModel
    {
        private readonly Assignment01.Context.MyDbContext _context;

        public DeleteModel(Assignment01.Context.MyDbContext context)
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

            var show = await _context.Shows.FirstOrDefaultAsync(m => m.ShowID == id);

            if (show == null)
            {
                return NotFound();
            }
            else 
            {
                Show = show;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Shows == null)
            {
                return NotFound();
            }
            var show = await _context.Shows.FindAsync(id);

            if (show != null)
            {
                Show = show;
                _context.Shows.Remove(Show);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
