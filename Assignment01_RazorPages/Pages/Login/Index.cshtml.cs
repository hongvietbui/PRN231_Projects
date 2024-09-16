using System.Text;
using System.Text.Json;
using Assignment01.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Assignment01.Pages.Login;

public class LoginModel : PageModel
{
    private readonly IHttpClientFactory _clientFactory;

    public LoginModel(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }
    
    [BindProperty]
    public LoginRequest LoginRequest { get; set; } = new LoginRequest();
    [BindProperty]
    public string ErrorMessage { get; set; } = "";
    public async Task<IActionResult> OnGetAsync()
    {
        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        var _httpClient = _clientFactory.CreateClient("CinemaAPI");
        var response = await _httpClient.PostAsync("api/Auth", new StringContent(JsonSerializer.Serialize(LoginRequest), Encoding.UTF8, "application/json"));
        if (response.IsSuccessStatusCode)
        {
            return Redirect("/Films/Index");
        }
        ErrorMessage = "Invalid username or password!";
        return Page();
    }
    
}