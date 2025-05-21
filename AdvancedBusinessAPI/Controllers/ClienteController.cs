using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdvancedBusinessAPI.Data;
using AdvancedBusinessAPI.Models;

namespace AdvancedBusinessAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClienteController : ControllerBase
{
    private readonly AppDbContext _context;

    public ClienteController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetClientes() =>
        Ok(await _context.Clientes.ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCliente(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        return cliente == null ? NotFound() : Ok(cliente);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCliente(Cliente cliente)
    {
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCliente(int id, Cliente cliente)
    {
        if (id != cliente.Id) return BadRequest();
        _context.Entry(cliente).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCliente(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null) return NotFound();
        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}