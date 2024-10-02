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

namespace Assignment01_RazorPages.Pages.Shows
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

      public ShowDTO Show { get; set; } = default!;
      public FilmResponseDTO FilmResponse { get; set; } = default!;
      public RoomDTO Room { get; set; } = default!;
 
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var response = await _httpClient.GetAsync($"odata/Show/{id}");
            Show = await response.Content.ReadFromJsonAsync<ShowDTO>();

            var filmResponse = await _httpClient.GetAsync($"odata/Film/{Show.FilmID}");
            FilmResponse = await filmResponse.Content.ReadFromJsonAsync<FilmResponseDTO>();

            var roomResponse = await _httpClient.GetAsync($"api/Room/{Show.RoomID}");
            Room = await roomResponse.Content.ReadFromJsonAsync<RoomDTO>();
            return Page();
        }
    }
}
