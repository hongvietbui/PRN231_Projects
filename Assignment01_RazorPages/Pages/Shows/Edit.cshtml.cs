using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assignment01.Context;
using Assignment01.DTO;
using Assignment01.Entities;

namespace Assignment01_RazorPages.Pages.Shows
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("CinemaAPI");
        }

        [BindProperty]
        public ShowDTO Show { get; set; } = default!;

        [BindProperty]
        public List<SelectListItem> RoomList { get; set; } = new List<SelectListItem>();
        [BindProperty]
        public List<SelectListItem> FilmList { get; set; } = new List<SelectListItem>();
        
        [BindProperty]
        public List<SelectListItem> StatusList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem
            {
                Text = "Active",
                Value = "true"
            },
            new SelectListItem
            {
                Text = "Inactive",
                Value = "false"
            }
        };

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var response = await _httpClient.GetAsync($"odata/Show/{id}");
            Show = await response.Content.ReadFromJsonAsync<ShowDTO>();
            
            var slotList = new SelectList(new List<Slot>(), "Value", "Name");
            var searchResponse = await _httpClient.GetAsync($"odata/Show?$filter=RoomID eq {Show.RoomID} and ShowDate eq {Show.ShowDate.ToString("yyyy-MM-dd")}");
            if (searchResponse.IsSuccessStatusCode)
            {
                var allSlots = Enumerable.Range(1, 9).ToList();

                var slotResponse = await searchResponse.Content.ReadFromJsonAsync<OdataAPIResp<List<ShowDTO>>>();
                var existingSlots = slotResponse?.Value.Select(show => show.Slot).ToList();
                var availableSlots = allSlots.Except(existingSlots).ToList();
                availableSlots.Add(Show.Slot);
            
                var slotSelectList = availableSlots.Select(s => new Slot
                {
                    Name = s.ToString(),
                    Value = s
                }).ToList();
                slotSelectList = slotSelectList.OrderBy(s => s.Value).ToList();
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

            var roomResponse = await _httpClient.GetAsync("api/Room");
            var roomList = await roomResponse.Content.ReadFromJsonAsync<List<RoomDTO>>();
            RoomList = roomList.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.RoomID.ToString()
            }).ToList();

            var filmResponse = await _httpClient.GetAsync("odata/Film");
            var filmListResp = await filmResponse.Content.ReadFromJsonAsync<OdataAPIResp<List<FilmResponseDTO>>>();
            var filmList = filmListResp?.Value;
            FilmList = filmList.Select(f => new SelectListItem
            {
                Text = f.Title,
                Value = f.FilmID.ToString()
            }).ToList();
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var response = await _httpClient.PutAsJsonAsync($"odata/Show/{Show.ShowID}", Show);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index", new { showDate = Show.ShowDate.ToString("yyyy-MM-dd"), selectedRoomId = Show.RoomID });

            }
            return Page();
        }
    }
}
