using Assignment01.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Assignment01.Pages.Films;

public class SearchModel : PageModel
{
    [BindProperty] 
    public List<FilmResponseDTO>? Films { get; set; } = new List<FilmResponseDTO>();
    //method 1
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;
    
    public SearchModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _httpClient = httpClientFactory.CreateClient("CinemaAPI");
    }
    
    public async Task<IActionResult> OnGetAsync(string title)
    {
        var response = await _httpClient.GetAsync($"odata/Film?$filter=contains(Title, '{title}')");
        
        if(response.IsSuccessStatusCode)
        {
            var respInfo = await response.Content.ReadFromJsonAsync<OdataAPIResp<List<FilmResponseDTO>>>();
            Films = respInfo?.Value;
        }

        return Page();
    }
}