using Assignment01.Context;
using Assignment01.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assignment01.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomController : ControllerBase
{
    private readonly MyDbContext _context;

    public RoomController(MyDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomDTO>>> GetRooms()
    {
        return await _context.Rooms.Select(r => new RoomDTO
        {
            RoomID = r.RoomID,
            Name = r.Name,
            NumberRows = r.NumberRows,
            NumberCols = r.NumberCols
        }).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoomDTO>> GetRoom(int id)
    {
        if (_context.Rooms == null)
            return NotFound();
        var roomEntity = await _context.Rooms.FindAsync(id);

        if (roomEntity == null)
            return NotFound();
        
        var roomDTO = new RoomDTO
        {
            RoomID = roomEntity.RoomID,
            Name = roomEntity.Name,
            NumberRows = roomEntity.NumberRows,
            NumberCols = roomEntity.NumberCols
        };

        return roomDTO;
    }
}