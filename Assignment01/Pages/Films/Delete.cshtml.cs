using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Assignment01.Context;
using Assignment01.Entities;

namespace Assignment01.Pages
{
    public class DeleteModel : PageModel
    {
        public Film Film { get; set; }
    }
}
