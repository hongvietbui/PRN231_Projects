using Assignment01.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;

namespace Assignment01.Pages.Login
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginModel(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _clientFactory = clientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public LoginRequest LoginRequest { get; set; } = new LoginRequest();

        [BindProperty]
        public string ErrorMessage { get; set; } = "";

        public async Task<IActionResult> OnGetAsync()
        {
            var isAuthenticated = HttpContext.Session.GetInt32("IsAuthenticated");

            if (isAuthenticated == 1) // Kiểm tra trạng thái đăng nhập từ Session
            {
                return Redirect("/Films/Index"); // Điều hướng về trang khác nếu đã đăng nhập
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //var _httpClient = _clientFactory.CreateClient("CinemaAPI");
            //var response = await _httpClient.PostAsync("api/Auth",
            //    new StringContent(JsonSerializer.Serialize(LoginRequest), Encoding.UTF8, "application/json"));

            //if (response.IsSuccessStatusCode)
            //{
            //    var responseContent = await response.Content.ReadAsStringAsync();
            //    var loginResult = JsonSerializer.Deserialize<LoginResult>(responseContent);

            //    // After a successful login, store the username in the Razor Pages session

            //    _httpContextAccessor.HttpContext.Session.SetString("Username", LoginRequest.Username);
            //    HttpContext.Session.SetInt32("IsAuthenticated", 1);
            //    var sessionValue = _httpContextAccessor.HttpContext.Session.GetString("Username");
            //    Console.WriteLine($"Session Username: {sessionValue}");
            //    // Optionally, store in TempData to pass to the next request
            //    TempData["Username"] = LoginRequest.Username;



            //    return Redirect("/Films/Index");
            //}

            //ErrorMessage = "Invalid username or password!";
            //return Page();
            var _httpClient = _clientFactory.CreateClient("CinemaAPI");
            var odataUrl = "https://localhost:7196/api/Auth"; // URL OData cho API

            var response = await _httpClient.PostAsync(odataUrl,
                new StringContent(JsonSerializer.Serialize(LoginRequest), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var loginResult = JsonSerializer.Deserialize<LoginResult>(responseContent);

                // Lưu trạng thái đăng nhập vào Session
                _httpContextAccessor.HttpContext.Session.SetString("Username", LoginRequest.Username);
                HttpContext.Session.SetInt32("IsAuthenticated", 1);

                // Lưu vào TempData để chuyển sang request tiếp theo
                TempData["Username"] = LoginRequest.Username;

                return Redirect("/Films/Index");
            }

            ErrorMessage = "Invalid username or password!";
            return Page();

        }
    }
    public class LoginResult
    {
        public string Username { get; set; }
        public string Message { get; set; }
    }
}
