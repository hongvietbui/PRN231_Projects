using System.Globalization;
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

    [BindProperty]
    public DateTime ShowDate { get; set; }

    public async Task<IActionResult> OnGetAsync(DateTime showDate, int selectedRoomId)
    {
        var isAuthenticated = HttpContext.Session.GetInt32("IsAuthenticated");

        if (isAuthenticated != 1) // Nếu người dùng chưa đăng nhập
        {
            return Redirect("/Login/Index"); // Điều hướng về trang đăng nhập
        }
        var roomResponse = await _httpClient.GetAsync("api/Room");
        var rooms = await roomResponse.Content.ReadFromJsonAsync<List<RoomDTO>>();
        ViewData["Rooms"] = new SelectList(rooms, "RoomID", "Name");

        var response = await _httpClient.GetAsync($"api/Show/search/{showDate.ToString("yyyy-MM-dd")}/{selectedRoomId}");
        if (response.IsSuccessStatusCode)
            ShowList = await response.Content.ReadFromJsonAsync<List<ShowDTO>>();
        else
        {
            ShowList = new List<ShowDTO>();
        }
        return Page();
    }
}