using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Assignment01_RazorPages.Pages.Logout
{
    public class IndexModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IndexModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult OnGet()
        {
            // Clear the session
            _httpContextAccessor.HttpContext.Session.Remove("Username");
            _httpContextAccessor.HttpContext.Session.Remove("IsAuthenticated");

            // Redirect to login page or home page
            return RedirectToPage("/Login/Index");
        }
    }
}
