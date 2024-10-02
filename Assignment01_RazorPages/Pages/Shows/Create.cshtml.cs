using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Assignment01.DTO;
using Assignment01.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Assignment01_RazorPages.Pages.Shows
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("CinemaAPI");
        }

        [BindProperty]
        public ShowDTO Show { get; set; } = new ShowDTO();

        [BindProperty]
        public DateTime DateNow { get; set; }

        [BindProperty]
        public int SelectedRoomId { get; set; }

        public List<int> AvailableSlots { get; set; } = new List<int>();

        public async Task<IActionResult> OnGetAsync(DateTime dateNow, int selectedRoomId)
        {
            var isAuthenticated = HttpContext.Session.GetInt32("IsAuthenticated");

            if (isAuthenticated != 1) // Nếu người dùng chưa đăng nhập
            {
                return Redirect("/Login/Index"); // Điều hướng về trang đăng nhập
            }
            // Set the date and room from the query parameters
            DateNow = dateNow;
            SelectedRoomId = selectedRoomId;

            // Fetch film and room lists
            var filmResponse = await _httpClient.GetAsync($"api/Film");
            var filmList = await filmResponse.Content.ReadFromJsonAsync<List<FilmResponseDTO>>();

            var roomResponse = await _httpClient.GetAsync($"api/Room");
            var roomList = await roomResponse.Content.ReadFromJsonAsync<List<RoomDTO>>();

            // Fetch available slots for the selected date
            var slotAvailableResponse = await _httpClient.GetAsync($"api/Show/getslot/{DateNow:yyyy-MM-dd}");
            AvailableSlots = await slotAvailableResponse.Content.ReadFromJsonAsync<List<int>>();

            // Populate dropdowns for films and rooms
            ViewData["FilmID"] = new SelectList(filmList, "FilmID", "Title");
            ViewData["RoomID"] = new SelectList(roomList, "RoomID", "RoomID");

            // Set the ShowDTO with the selected date and room
            Show = new ShowDTO { ShowDate = DateNow, RoomID = SelectedRoomId };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Show == null)
            {
                return Page();
            }

            // Post the new show data to the API
            var response = await _httpClient.PostAsJsonAsync("api/Show", Show);
            if (response.IsSuccessStatusCode)
            {
                // Redirect to the index page, with selected room and show date parameters
                return RedirectToPage("/Shows/Index", new
                {
                    showDate = Show.ShowDate.ToString("yyyy-MM-dd"),
                    selectedRoomId = Show.RoomID
                });
            }

            // If the response fails, redisplay the form with validation errors
            return Page();
        }
    }
}
