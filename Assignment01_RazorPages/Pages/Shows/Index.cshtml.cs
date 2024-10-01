using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Assignment01.Context;
using Assignment01.DTO;
using Assignment01.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public IList<ShowDTO> Shows { get;set; } = default!;

        [BindProperty]
        public DateTime ShowDate { get; set; } = DateTime.Now;
        
        [BindProperty]
        public int? SelectedRoomId { get; set; } = 1;
        

        public async Task<IActionResult> OnGetAsync(DateTime? showDate, int? selectedRoomId)
        {
            if(showDate!=null)
                ShowDate = showDate.Value;
            if(selectedRoomId!=null)
                SelectedRoomId = selectedRoomId.Value;
            var response = await _httpClient.GetAsync($"odata/Show?$filter=ShowDate eq {ShowDate.ToString("yyyy-MM-dd")} and RoomID eq {SelectedRoomId}");
            var odataResp = await response.Content.ReadFromJsonAsync<OdataAPIResp<IList<ShowDTO>>>();
            Shows = odataResp?.Value ?? new List<ShowDTO>(); 

            var roomResponse = await _httpClient.GetAsync("api/Room");
            var rooms = await roomResponse.Content.ReadFromJsonAsync<List<RoomDTO>>();
            ViewData["Rooms"] = new SelectList(rooms, "RoomID", "Name");
            
            return Page();
        }
    }
}
