using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Assignment01.DTO.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Assignment01.Entities;

namespace Assignment01.Pages.Films
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _client;

        public CreateModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _client = _clientFactory.CreateClient("CinemaAPI");
        }

        [BindProperty] public FilmDTO Film { get; set; } = default!;
        
        [BindProperty]
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var genreResponse = await _client.GetAsync("api/Genre");
            var genreList = await genreResponse.Content.ReadFromJsonAsync<List<Genre>>();
            ViewData["GenreList"] = new SelectList(genreList, "GenreID", "Name");
            
            var countryResponse = await _client.GetAsync("api/Country");
            var countryList = await countryResponse.Content.ReadFromJsonAsync<List<Country>>();
            ViewData["CountryList"] = new SelectList(countryList, "CountryCode", "CountryName");
            
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            // If the model state is not valid, return the page
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Error creating film";
                await OnGetAsync();
            }
            
            var response = await _client.PostAsync("api/film", new StringContent(JsonSerializer.Serialize(Film), Encoding.UTF8, "application/json"));
            // If the response is successful, redirect to the index page
            if(response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }
            ErrorMessage = "Error creating film";
            return Page();
        }
    }
}
