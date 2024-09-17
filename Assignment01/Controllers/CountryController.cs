using Assignment01.Context;
using Assignment01.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assignment01.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountryController : ControllerBase
{
    private readonly MyDbContext _context;

    public CountryController(MyDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<CountryDTO>>> GetCountries()
    {
        if (_context.Countries == null)
            return NotFound();

        return await _context.Countries.Select(c => new CountryDTO
        {
            CountryCode = c.CountryCode,
            CountryName = c.CountryName
        }).ToListAsync();
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<CountryDTO>> GetCountry(string id)
    {
        if (_context.Countries == null)
            return NotFound();

        var country = await _context.Countries.FindAsync(id);

        if (country == null)
            return NotFound();

        return new CountryDTO
        {
            CountryCode = country.CountryCode,
            CountryName = country.CountryName
        };
    }
}