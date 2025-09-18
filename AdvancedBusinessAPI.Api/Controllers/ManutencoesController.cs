using AdvancedBusinessAPI.Domain;
using AdvancedBusinessAPI.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace AdvancedBusinessAPI.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[SwaggerTag("Gestão de Manutenções (CRUD e consulta por Moto).")]
public class ManutencoesController(AppDbContext db) : ControllerBase
{
  /// <summary>Listar manutenções</summary>
  /// <remarks>
  /// Use <code>?motoId=GUID</code> para filtrar por uma moto.  
  /// Ordenação padrão: mais recentes primeiro.
  /// </remarks>
    [HttpGet]
  [SwaggerOperation(
    Summary = "Listar manutenções",
    Description = "Lista manutenções; pode filtrar por motoId. Ordena por data desc."
  )]
  [SwaggerResponse(200, "Lista de manutenções")]
    public async Task<IActionResult> Get([FromQuery] Guid? motoId = null)
    {
        IQueryable<Manutencao> q = db.Manutencoes.AsNoTracking();
        if (motoId is { } id) q = q.Where(x => x.MotoId == id);
        var list = await q.OrderByDescending(x => x.Data).ToListAsync();
        return Ok(list);
    }

  /// <summary>Detalhar manutenção</summary>
    [HttpGet("{id:guid}")]
  [SwaggerOperation(
    Summary = "Obter manutenção por ID",
    Description = "Retorna a manutenção correspondente ao ID."
  )]
  [SwaggerResponse(200, "Manutenção encontrada")]
  [SwaggerResponse(404, "Manutenção não encontrada")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var item = await db.Manutencoes.FindAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

  /// <summary>Criar manutenção</summary>
  /// <remarks>
  /// Exige <code>motoId</code> válido.  
  /// Exemplo:
  /// <code>
  /// {
  ///   "motoId":"GUID","data":"2025-09-18T00:00:00Z",
  ///   "tipo":"Revisao","descricao":"Troca de óleo","status":"Pendente","custo":150.0
  /// }
  /// </code>
  /// </remarks>
    [HttpPost]
  [SwaggerOperation(
    Summary = "Criar manutenção",
    Description = "Cria uma manutenção para uma moto existente."
  )]
  [SwaggerResponse(201, "Criado")]
  [SwaggerResponse(422, "Moto inexistente")]
  [SwaggerResponse(400, "Dados inválidos")]
    public async Task<IActionResult> Post([FromBody] Manutencao input)
    {
        if (!await db.Motos.AnyAsync(m => m.Id == input.MotoId))
            return UnprocessableEntity(new { error = "Moto inexistente." });

        input.Id = Guid.NewGuid();
        await db.Manutencoes.AddAsync(input);
        await db.SaveChangesAsync();
        return Created($"/api/v1/manutencoes/{input.Id}", new { input.Id });
    }

  /// <summary>Atualizar manutenção</summary>
    [HttpPut("{id:guid}")]
  [SwaggerOperation(
    Summary = "Atualizar manutenção",
    Description = "Atualiza os dados da manutenção. Retorna 204 em sucesso."
  )]
  [SwaggerResponse(204, "Atualizado")]
  [SwaggerResponse(404, "Manutenção não encontrada")]
  [SwaggerResponse(400, "Dados inválidos")]
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

  /// <summary>Excluir manutenção</summary>
    [HttpDelete("{id:guid}")]
  [SwaggerOperation(
    Summary = "Excluir manutenção",
    Description = "Remove a manutenção pelo ID."
  )]
  [SwaggerResponse(204, "Excluída")]
  [SwaggerResponse(404, "Manutenção não encontrada")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var item = await db.Manutencoes.FindAsync(id);
        if (item is null) return NotFound();
        db.Manutencoes.Remove(item);
        await db.SaveChangesAsync();
        return NoContent();
    }
    
  /// <summary>Listar manutenções de uma moto</summary>
  /// <remarks>Sub-recurso: retorna todas as manutenções de uma moto existente.</remarks>
    [HttpGet("/api/v1/motos/{motoId:guid}/manutencoes")]
  [SwaggerOperation(
    Summary = "Manutenções por moto",
    Description = "Lista as manutenções vinculadas à moto informada."
  )]
  [SwaggerResponse(200, "Lista de manutenções da moto")]
  [SwaggerResponse(404, "Moto não encontrada")]
    public async Task<IActionResult> GetByMoto(Guid motoId)
    {
        if (!await db.Motos.AnyAsync(m => m.Id == motoId)) return NotFound(new { error = "Moto não encontrada" });
        var list = await db.Manutencoes.AsNoTracking().Where(x => x.MotoId == motoId)
            .OrderByDescending(x => x.Data).ToListAsync();
        return Ok(list);
    }
}
