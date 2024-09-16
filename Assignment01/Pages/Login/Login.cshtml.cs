using Assignment01.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Assignment01.Pages.Login;

public class Login : PageModel
{
    [BindProperty]
    public LoginRequest LoginModel { get; set; } = new LoginRequest();
    public void OnGet()
    {
        
    }
}