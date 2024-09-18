using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Assignment01.Context;
using Assignment01.DTO;
using Assignment01.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Assignment01_RazorPages.Pages.Shows
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("CinemaAPI");
        }
        
        [BindProperty]
        public IList<ShowDTO> Shows { get;set; } = default!;

        [BindProperty]
        public DateTime ShowDate { get; set; } = DateTime.Now;

        public async Task<IActionResult> OnGetAsync()
        {
            var response = await _httpClient.GetAsync("api/Show");
            Shows = await response.Content.ReadFromJsonAsync<List<ShowDTO>>();

            var roomResponse = await _httpClient.GetAsync("api/Room");
            var rooms = await roomResponse.Content.ReadFromJsonAsync<List<RoomDTO>>();
            ViewData["Rooms"] = new SelectList(rooms, "RoomID", "Name");
            return Page();
        }
    }
}
