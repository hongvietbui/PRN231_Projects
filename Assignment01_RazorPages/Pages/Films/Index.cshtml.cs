using System.Text.Json;
using Assignment01.DTO;
using Assignment01.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Assignment01.Pages.Films
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
        public List<FilmResponseDTO>? Films { get; set; } = new List<FilmResponseDTO>();
        
        public async Task<IActionResult> OnGetAsync()
        {
            var isAuthenticated = HttpContext.Session.GetInt32("IsAuthenticated");

            if (isAuthenticated != 1) // Nếu người dùng chưa đăng nhập
            {
                return Redirect("/Login/Index"); // Điều hướng về trang đăng nhập
            }
            var response = await _httpClient.GetAsync("api/Film");
            
            if(response.IsSuccessStatusCode)
            {
                Films = await response.Content.ReadFromJsonAsync<List<FilmResponseDTO>>();
            }
            
            return Page();
        }
    }
}
