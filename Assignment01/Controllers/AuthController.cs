using Assignment01.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Extensions.Configuration;

namespace Assignment01.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ODataController
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    public IActionResult Login([FromBody] LoginRequest userInfo)
    {
        //var user = _configuration.GetSection("UserSettings")["User"];
        //var password = _configuration.GetSection("UserSettings")["Password"];

        //if (userInfo.Username == user && userInfo.Password == password)
        //{
        //    // Store username in session after successful login

        //    return Ok(new { Username = userInfo.Username, Message = "Login successful" });
        //}

        //return Unauthorized();
        var user = _configuration.GetSection("UserSettings")["User"];
        var password = _configuration.GetSection("UserSettings")["Password"];

        if (userInfo.Username == user && userInfo.Password == password)
        {
            // Trả về kết quả với OData
            return Ok(new { Username = userInfo.Username, Message = "Login successful" });
        }

        return Unauthorized();
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        _httpContextAccessor.HttpContext.Session.Remove("Username");
        return Ok(new { Message = "Logged out successfully" });
    }
}
