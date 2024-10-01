using System.Text.Json;
using Assignment01.DTO;
using Assignment01.DTO.Request;
using Assignment01.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Assignment01.Pages.Films
{
    public class IndexModel : PageModel
    {
        //method 1
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("CinemaAPI");
        }
        
        //method 2
        // private readonly HttpClient _httpClient;
        // public IndexModel(HttpClient httpClient)
        // {
        //     _httpClient = httpClient;
        // }
        
        [BindProperty] 
        public List<FilmResponseDTO>? Films { get; set; } = new List<FilmResponseDTO>();
        
        public async Task<IActionResult> OnGetAsync()
        {
            var response = await _httpClient.GetAsync("odata/Film");
            
            if(response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadFromJsonAsync<OdataAPIResp<List<FilmResponseDTO>>>();
                Films = resp?.Value;
            }
            
            return Page();
        }
    }
}
