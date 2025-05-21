using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdvancedBusinessAPI.Data;
using AdvancedBusinessAPI.Models;

namespace AdvancedBusinessAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MotoController : ControllerBase
{
    private readonly AppDbContext _context;

    public MotoController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Moto>>> GetMotos()
    {
        return await _context.Motos.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Moto>> GetMoto(int id)
    {
        var moto = await _context.Motos.FindAsync(id);

        if (moto == null)
        {
            return NotFound();
        }

        return moto;
    }

    [HttpPost]
    public async Task<ActionResult<Moto>> PostMoto(Moto moto)
    {
        _context.Motos.Add(moto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMoto), new { id = moto.Id }, moto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutMoto(int id, Moto moto)
    {
        if (id != moto.Id)
        {
            return BadRequest();
        }

        _context.Entry(moto).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMoto(int id)
    {
        var moto = await _context.Motos.FindAsync(id);
        if (moto == null)
        {
            return NotFound();
        }

        _context.Motos.Remove(moto);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
