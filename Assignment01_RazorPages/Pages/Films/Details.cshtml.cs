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
    public class DetailsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        
        public DetailsModel(IHttpClientFactory httpClientFactory)
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

            var response = await _httpClient.GetAsync($"odata/Film/{id}");
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
    }
}
