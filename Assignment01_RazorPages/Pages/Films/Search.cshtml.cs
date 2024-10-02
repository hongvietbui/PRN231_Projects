using Assignment01.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Assignment01.Pages.Films;

public class SearchModel : PageModel
{
    [BindProperty] 
    public List<FilmResponseDTO>? Films { get; set; } = new List<FilmResponseDTO>();
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;
    
    public SearchModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _httpClient = httpClientFactory.CreateClient("CinemaAPI");
    }
    
    public async Task<IActionResult> OnGetAsync(string title)
    {
        var isAuthenticated = HttpContext.Session.GetInt32("IsAuthenticated");

        if (isAuthenticated != 1) // Nếu người dùng chưa đăng nhập
        {
            return Redirect("/Login/Index"); // Điều hướng về trang đăng nhập
        }
        var response = await _httpClient.GetAsync($"api/Film/search/{title}");
        
        if(response.IsSuccessStatusCode)
        {
            Films = await response.Content.ReadFromJsonAsync<List<FilmResponseDTO>>();
        }

        return Page();
    }
}