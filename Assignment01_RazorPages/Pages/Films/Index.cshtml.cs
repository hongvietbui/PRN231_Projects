using System.Text.Json;
using Assignment01.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Assignment01.Pages.Films
{
    public class IndexModel : PageModel
    {
        [BindProperty] 
        public List<Film>? Films { get; set; } = new List<Film>();
        //method 1
        private readonly IHttpClientFactory _httpClientFactory;
        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        
        //method 2
        // private readonly HttpClient _httpClient;
        // public IndexModel(HttpClient httpClient)
        // {
        //     _httpClient = httpClient;
        // }
        
        public async Task OnGetAsync()
        {
            //Method 1:
            var httpClient = _httpClientFactory.CreateClient("CinemaAPI");
            
            using HttpResponseMessage response = await httpClient.GetAsync("api/Film");
            
            if(response.IsSuccessStatusCode)
            {
                // var contentStr = await response.Content.ReadAsStringAsync();
                // var content = await response.Content.ReadAsStreamAsync();
                //
                // // Films = await JsonSerializer.DeserializeAsync<List<Film>>(content);
                // Films = JsonSerializer.Deserialize<List<Film>>(contentStr);
                Films = await response.Content.ReadFromJsonAsync<List<Film>>();
            }

            //Method 2
            // _httpClient.BaseAddress = new Uri("https://localhost:7196/");
            // var response = await _httpClient.GetAsync("api/Film");
            //
            // response.EnsureSuccessStatusCode();
            //
            // Films = await response.Content.ReadFromJsonAsync<List<Film>>();
        }
    }
}
