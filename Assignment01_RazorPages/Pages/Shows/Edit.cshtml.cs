using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assignment01.Context;
using Assignment01.DTO;
using Assignment01.Entities;

namespace Assignment01_RazorPages.Pages.Shows
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("CinemaAPI");
        }

        [BindProperty]
        public ShowDTO Show { get; set; } = default!;

        [BindProperty]
        public List<SelectListItem> RoomList { get; set; } = new List<SelectListItem>();
        [BindProperty]
        public List<SelectListItem> FilmList { get; set; } = new List<SelectListItem>();
        
        [BindProperty]
        public List<SelectListItem> StatusList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem
            {
                Text = "Active",
                Value = "true"
            },
            new SelectListItem
            {
                Text = "Inactive",
                Value = "false"
            }
        };

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var response = await _httpClient.GetAsync($"api/Show/{id}");
            Show = await response.Content.ReadFromJsonAsync<ShowDTO>();

            var roomResponse = await _httpClient.GetAsync("api/Room");
            var roomList = await roomResponse.Content.ReadFromJsonAsync<List<RoomDTO>>();
            RoomList = roomList.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.RoomID.ToString()
            }).ToList();

            var filmResponse = await _httpClient.GetAsync("api/Film");
            var filmList = await filmResponse.Content.ReadFromJsonAsync<List<FilmResponseDTO>>();
            
            FilmList = filmList.Select(f => new SelectListItem
            {
                Text = f.Title,
                Value = f.FilmID.ToString()
            }).ToList();
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Show/{Show.ShowID}", Show);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }
            return Page();
        }
    }
}
