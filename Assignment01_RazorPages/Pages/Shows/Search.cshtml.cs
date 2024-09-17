using Assignment01.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Assignment01_RazorPages.Pages.Shows;

public class SearchModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _httpClient;

    public SearchModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient("CinemaAPI");
    }
    
    [BindProperty]
    public List<ShowDTO> ShowList { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(DateTime showDate, int selectedRoomId)
    {
        var roomResponse = await _httpClient.GetAsync("api/Room");
        var rooms = await roomResponse.Content.ReadFromJsonAsync<List<RoomDTO>>();
        ViewData["Rooms"] = new SelectList(rooms, "RoomID", "Name");
        
        var response = await _httpClient.GetAsync($"api/Show/search/{showDate}/{selectedRoomId}");
        if(response.IsSuccessStatusCode)
            ShowList = await response.Content.ReadFromJsonAsync<List<ShowDTO>>();
        else
        {
            ShowList = new List<ShowDTO>();
        }
        return Page();
    }
}