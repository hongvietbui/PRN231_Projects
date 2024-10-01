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
using Assignment01.DTO.Request;
using Assignment01.Entities;

namespace Assignment01.Pages.Films
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
        public FilmDTO Film { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var response = await _httpClient.GetAsync($"odata/Film/{id}");
            Film = await response.Content.ReadFromJsonAsync<FilmDTO>();

            var genreResponse = await _httpClient.GetAsync("api/Genre");
            var genreList =await genreResponse.Content.ReadFromJsonAsync<List<GenreDTO>>();
            ViewData["GenreList"] = new SelectList(genreList, "GenreID", "Name");


            var countryResponse = await _httpClient.GetAsync("api/Country");
            var countryList =await countryResponse.Content.ReadFromJsonAsync<List<CountryDTO>>();
            ViewData["CountryList"] = new SelectList(countryList, "CountryCode", "CountryName");
            
            
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var response = await _httpClient.PutAsJsonAsync($"odata/Film/{Film.FilmID}", Film);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
