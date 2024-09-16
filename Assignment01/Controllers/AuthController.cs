using Assignment01.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Assignment01.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
        // _configuration = new ConfigurationBuilder()
        //     .SetBasePath(Directory.GetCurrentDirectory())
        //     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //     .Build();
    }
    //post: Login
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequest userInfo) 
    {
        //Get user and password from appsettings.json
        var user = _configuration.GetSection("UserSettings")["User"];
        var password = _configuration.GetSection("UserSettings")["Password"];
        if (userInfo.Username == user && userInfo.Password == password)
        {
            return Ok();
        }
        return Unauthorized();
    }
}