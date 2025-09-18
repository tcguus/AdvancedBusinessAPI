using AdvancedBusinessAPI.Domain;
using AdvancedBusinessAPI.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdvancedBusinessAPI.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ManutencoesController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] Guid? motoId = null)
    {
        IQueryable<Manutencao> q = db.Manutencoes.AsNoTracking();
        if (motoId is { } id) q = q.Where(x => x.MotoId == id);
        var list = await q.OrderByDescending(x => x.Data).ToListAsync();
        return Ok(list);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var item = await db.Manutencoes.FindAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Manutencao input)
    {
        if (!await db.Motos.AnyAsync(m => m.Id == input.MotoId))
            return UnprocessableEntity(new { error = "Moto inexistente." });

        input.Id = Guid.NewGuid();
        await db.Manutencoes.AddAsync(input);
        await db.SaveChangesAsync();
        return Created($"/api/v1/manutencoes/{input.Id}", new { input.Id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] Manutencao input)
    {
        var item = await db.Manutencoes.FindAsync(id);
        if (item is null) return NotFound();

        item.Data = input.Data;
        item.Tipo = input.Tipo;
        item.Descricao = input.Descricao;
        item.Status = input.Status;
        item.Custo = input.Custo;

        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var item = await db.Manutencoes.FindAsync(id);
        if (item is null) return NotFound();
        db.Manutencoes.Remove(item);
        await db.SaveChangesAsync();
        return NoContent();
    }

    // sub-recurso: GET /api/v1/motos/{id}/manutencoes
    [HttpGet("/api/v1/motos/{motoId:guid}/manutencoes")]
    public async Task<IActionResult> GetByMoto(Guid motoId)
    {
        if (!await db.Motos.AnyAsync(m => m.Id == motoId)) return NotFound(new { error = "Moto não encontrada" });
        var list = await db.Manutencoes.AsNoTracking().Where(x => x.MotoId == motoId)
            .OrderByDescending(x => x.Data).ToListAsync();
        return Ok(list);
    }
}
