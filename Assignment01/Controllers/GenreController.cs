using Assignment01.Context;
using Assignment01.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assignment01.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class GenreController : ControllerBase
{
    private readonly MyDbContext _context;

    public GenreController(MyDbContext context)
    {
        _context = context;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<GenreDTO>>> GetGenres()
    {
        if (_context.Genres == null)
        {
            return NotFound();
        }
        return await _context.Genres.Select(genre => new GenreDTO
        {
            GenreID = genre.GenreID,
            Name = genre.Name
        }).ToListAsync();
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<GenreDTO>> GetGenre(int id)
    {
        if (_context.Genres == null)
        {
            return NotFound();
        }
        var genre = await _context.Genres.FindAsync(id);

        if (genre == null)
        {
            return NotFound();
        }

        return new GenreDTO
        {
            GenreID = genre.GenreID,
            Name = genre.Name
        };
    }
}