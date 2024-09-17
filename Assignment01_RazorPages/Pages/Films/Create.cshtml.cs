using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Assignment01.Entities;

namespace Assignment01.Pages.Films
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _client;

        public CreateModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _client = _clientFactory.CreateClient("CinemaAPI");
        }

        [BindProperty]
        public Film Film { get; set; }
        
        [BindProperty]
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            // If the model state is not valid, return the page

            
            var response = _client.PostAsync("api/film", new StringContent(JsonSerializer.Serialize(Film), Encoding.UTF8, "application/json")).Result;
            // If the response is successful, redirect to the index page
            if(response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }
            ErrorMessage = "Error creating film";
            return Page();
        }
    }
}
