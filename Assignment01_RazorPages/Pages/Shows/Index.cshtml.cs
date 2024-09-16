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
    public class IndexModel : PageModel
    {
        private readonly Assignment01.Context.MyDbContext _context;

        public IndexModel(Assignment01.Context.MyDbContext context)
        {
            _context = context;
        }

        public IList<Show> Show { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Shows != null)
            {
                Show = await _context.Shows
                .Include(s => s.Film)
                .Include(s => s.Room).ToListAsync();
            }
        }
    }
}
