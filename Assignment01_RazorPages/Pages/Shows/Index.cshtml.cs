using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Assignment01.DTO;
using Assignment01.Entities;

namespace Assignment01_RazorPages.Pages.Shows
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
        public IList<ShowDTO> Shows { get; set; } = new List<ShowDTO>();

        [BindProperty]
        public DateTime DateNow { get; set; } = DateTime.Now;

        [BindProperty]
        public int SelectedRoomId { get; set; }

        public async Task<IActionResult> OnGetAsync(DateTime? showDate, int? selectedRoomId =1)
        {
            var isAuthenticated = HttpContext.Session.GetInt32("IsAuthenticated");

            if (isAuthenticated != 1) // Nếu người dùng chưa đăng nhập
            {
                return Redirect("/Login/Index"); // Điều hướng về trang đăng nhập
            }
            // Load room data
            var roomResponse = await _httpClient.GetAsync("api/Room");
            var rooms = await roomResponse.Content.ReadFromJsonAsync<List<RoomDTO>>();
            ViewData["Rooms"] = new SelectList(rooms, "RoomID", "Name");

            // If no search parameters are provided, use the current date and show all shows
            if (!showDate.HasValue) showDate = DateTime.Now;
            if (!selectedRoomId.HasValue) selectedRoomId = 0;

            DateNow = showDate.Value;
            SelectedRoomId = selectedRoomId.Value;

            // Call the search API with the selected date and room
            var searchResponse = await _httpClient.GetAsync($"api/Show/search/{DateNow.ToString("yyyy-MM-dd")}/{SelectedRoomId}");
            if (searchResponse.IsSuccessStatusCode)
            {
                Shows = await searchResponse.Content.ReadFromJsonAsync<List<ShowDTO>>();
            }

            return Page();
        }
    }
}
