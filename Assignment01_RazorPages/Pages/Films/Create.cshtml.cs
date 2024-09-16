using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Assignment01.Entities;

namespace Assignment01.Pages.Films
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Film Film { get; set; }
    }
}
