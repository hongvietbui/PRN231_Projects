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
using Microsoft.VisualBasic;

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

        public async Task<IActionResult> OnGetAsync(DateTime? showDate, int? selectedRoomId = 1)
        {
            var slotList = new SelectList(new List<Slot>(), "Value", "Name");
            Show.ShowDate = showDate ?? DateTime.Today;
            Show.RoomID = selectedRoomId ?? 1;

            var response = await _httpClient.GetAsync($"odata/Show?$filter=RoomID eq {selectedRoomId} and ShowDate eq {Show.ShowDate.ToString("yyyy-MM-dd")}");
            if (response.IsSuccessStatusCode)
            {
                var allSlots = Enumerable.Range(1, 9).ToList();

                var slotResponse = await response.Content.ReadFromJsonAsync<OdataAPIResp<List<ShowDTO>>>();
                var existingSlots = slotResponse?.Value.Select(show => show.Slot).ToList();
                var availableSlots = allSlots.Except(existingSlots).ToList();
            
                var slotSelectList = availableSlots.Select(s => new Slot
                {
                    Name = s.ToString(),
                    Value = s
                }).ToList();
                slotList = new SelectList(slotSelectList, "Value", "Name");
            }else
            {
                var slotSelectList = Enumerable.Range(1, 9).Select(s => new Slot
                {
                    Name = s.ToString(),
                    Value = s
                }).ToList();
                slotList = new SelectList(slotSelectList, "Value", "Name");
            }

            ViewData["SlotList"] = slotList;

            var filmResponse = await _httpClient.GetAsync($"odata/Film");
            var filmListResp = await filmResponse.Content.ReadFromJsonAsync<OdataAPIResp<List<FilmResponseDTO>>>();
            var filmList = filmListResp?.Value ?? new List<FilmResponseDTO>();

            var roomResposne = await _httpClient.GetAsync($"api/Room");
            var roomList = await roomResposne.Content.ReadFromJsonAsync<List<RoomDTO>>();

            ViewData["FilmID"] = new SelectList(filmList, "FilmID", "Title");
            ViewData["RoomID"] = new SelectList(roomList, "RoomID", "RoomID");
            ViewData["StatusList"] = new List<SelectListItem>
            {
                new SelectListItem { Text = "True", Value = "True" },
                new SelectListItem { Text = "False", Value = "False" }
            };
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

            var response = await _httpClient.PostAsJsonAsync("odata/Show", Show);
            if (response.IsSuccessStatusCode)
            {
                return Redirect($"/Shows/Index?showDate={Show.ShowDate.ToString("yyyy-MM-dd")}&selectedRoomId={Show.RoomID}");
            }

            return RedirectToPage("./Index");
        }
    }
}
