using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Assignment01.Context;
using Assignment01.DTO;
using Assignment01.DTO.Request;
using Assignment01.Entities;

namespace Assignment01.Pages.Films
{
    public class DeleteModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        
        public DeleteModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("CinemaAPI");
        }
        
        public FilmResponseDTO Film { get; set; }
        
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _httpClient.GetAsync($"api/Film/{id}");
            var filmDto = await response.Content.ReadFromJsonAsync<FilmDTO>();

            var genreResponse = await _httpClient.GetAsync($"api/Genre/{filmDto.GenreID}");
            var genreDto = await genreResponse.Content.ReadFromJsonAsync<GenreDTO>();
            
            var countryResponse = await _httpClient.GetAsync($"api/Country/{filmDto.CountryCode}");
            var countryDto = await countryResponse.Content.ReadFromJsonAsync<CountryDTO>();
            
            Film = new FilmResponseDTO
            {
                FilmID = filmDto.FilmID,
                Genre = genreDto.Name,
                Title = filmDto.Title,
                Year = filmDto.Year,
                CountryName = countryDto.CountryName,
                FilmUrl = filmDto.FilmUrl
            };
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _httpClient.DeleteAsync($"api/Film/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }
            return Page();
        }
    }
}
