using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Assignment01.Context;
using Assignment01.DTO;
using Assignment01.Entities;

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

        public async Task<IActionResult> OnGetAsync()
        {
            Show.ShowDate = DateTime.Today;

            var slotResponse = await _httpClient.GetAsync($"api/Show/getslot/{DateTime.Today.ToString("yyyy-MM-dd")}");
            var slotList = await slotResponse.Content.ReadFromJsonAsync<List<int>>();
            var slotSelectList = slotList.Select(s => new Slot
            {
                Name = s.ToString(),
                Value = s
            }).ToList();

            ViewData["SlotList"] = new SelectList(slotSelectList, "Value", "Name");

            var filmResponse = await _httpClient.GetAsync($"api/Film");
            var filmList = await filmResponse.Content.ReadFromJsonAsync<List<FilmResponseDTO>>();

            var roomResposne = await _httpClient.GetAsync($"api/Room");
            var roomList = await roomResposne.Content.ReadFromJsonAsync<List<RoomDTO>>();

            ViewData["FilmID"] = new SelectList(filmList, "FilmID", "Title");
            ViewData["RoomID"] = new SelectList(roomList, "RoomID", "RoomID");
            return Page();
        }

        [BindProperty]
        public ShowDTO Show { get; set; } = new ShowDTO();


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Show == null)
            {
                return Page();
            }

            var response = await _httpClient.PostAsJsonAsync("api/Show", Show);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }

            return RedirectToPage($"./Search?showDate={DateTime.Now.ToString("yyyy-MM-dd")}&selectRoomId={Show.RoomID}");
        }
    }
}
