using AdvancedBusinessAPI.Domain;
using AdvancedBusinessAPI.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Routing;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using AdvancedBusinessAPI.Api.SwaggerExamples;
using AdvancedBusinessAPI.Application.DTOs;



namespace AdvancedBusinessAPI.Api.Controllers;

[ApiController]
[SwaggerTag("Gestão de Motos")]
[Route("api/v1/[controller]")]
public class MotosController(AppDbContext db, LinkGenerator linkGen) : ControllerBase
{
    [HttpGet]
    /// <summary>Listar motos</summary>
    /// <remarks>
    /// Use <code>?page=1&amp;pageSize=20</code> para paginação.  
    /// Use <code>?pageSize=0</code> para retornar **todas** as motos.  
    /// Filtros disponíveis: <code>?modelo=</code> e <code>?placa=</code>.
    /// </remarks>
    [HttpGet]
    [SwaggerOperation(Summary = "Listar motos", Description = "Lista motos com filtros e paginação. Retorna HATEOAS.")]
    [SwaggerResponse(200, "Lista de motos")]
    [SwaggerResponseExample(200, typeof(MotosListResponseExample))]
    public async Task<IActionResult> Get([FromQuery] string? modelo, [FromQuery] string? placa,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
        [FromQuery] string sortBy = "modelo", [FromQuery] string sortDir = "asc")
    {
        IQueryable<Moto> q = db.Motos.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(modelo))
            q = q.Where(m => m.Modelo.Contains(modelo));
        if (!string.IsNullOrWhiteSpace(placa))
            q = q.Where(m => m.Placa.Contains(placa));

        // ordenação simples
        q = (sortBy.ToLower(), sortDir.ToLower()) switch
        {
            ("placa", "desc") => q.OrderByDescending(m => m.Placa),
            ("placa", _)      => q.OrderBy(m => m.Placa),
            ("ano", "desc")   => q.OrderByDescending(m => m.Ano),
            ("ano", _)        => q.OrderBy(m => m.Ano),
            ("modelo", "desc")=> q.OrderByDescending(m => m.Modelo),
            _                 => q.OrderBy(m => m.Modelo),
        };

        // se pageSize=0, devolve tudo (atende teu pedido)
        if (pageSize == 0)
        {
            var all = await q.ToListAsync();
            return Ok(new {
                items = all.Select(ToResource),
                links = new [] { SelfLink() }
            });
        }

        // paginação
        var total = await q.CountAsync();
        var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        // links HATEOAS
        var lastPage = Math.Max(1, (int)Math.Ceiling(total / (double)pageSize));
        var navLinks = new List<object> { SelfLink(page, pageSize) };
        if (page > 1) navLinks.Add(new { rel = "prev", href = UrlFor(page - 1, pageSize), method = "GET" });
        if (page < lastPage) navLinks.Add(new { rel = "next", href = UrlFor(page + 1, pageSize), method = "GET" });
        navLinks.Add(new { rel = "last", href = UrlFor(lastPage, pageSize), method = "GET" });

        Response.Headers["X-Pagination"] = System.Text.Json.JsonSerializer.Serialize(new { total, page, pageSize, lastPage });

        return Ok(new {
            items = items.Select(ToResource),
            navLinks
        });

        object ToResource(Moto m) => new {
            m.Id, m.Placa, m.Modelo, m.Ano, m.Status, m.Latitude, m.Longitude,
            links = new [] {
                new { rel="self", href=$"/api/v1/motos/{m.Id}", method="GET" },
                new { rel="update", href=$"/api/v1/motos/{m.Id}", method="PUT" },
                new { rel="delete", href=$"/api/v1/motos/{m.Id}", method="DELETE" },
                new { rel="manutencoes", href=$"/api/v1/motos/{m.Id}/manutencoes", method="GET" }
            }
        };

        object SelfLink(int p=1, int ps=20) => new { rel="self", href=UrlFor(p,ps), method="GET" };
        string UrlFor(int p, int ps) => linkGen.GetPathByAction(HttpContext, action: "Get", controller: "Motos",
            values: new { page = p, pageSize = ps, modelo, placa }) ?? $"/api/v1/motos?page={p}&pageSize={ps}";

    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var m = await db.Motos.FindAsync(id);
        if (m is null) return NotFound();
        return Ok(new {
            m.Id, m.Placa, m.Modelo, m.Ano, m.Status, m.Latitude, m.Longitude,
            links = new [] {
                new { rel="self", href=$"/api/v1/motos/{m.Id}", method="GET" },
                new { rel="update", href=$"/api/v1/motos/{m.Id}", method="PUT" },
                new { rel="delete", href=$"/api/v1/motos/{m.Id}", method="DELETE" },
                new { rel="manutencoes", href=$"/api/v1/motos/{m.Id}/manutencoes", method="GET" }
            }
        });
    }

    [HttpPost]
    /// <summary>Criar moto</summary>
    [HttpPost]
    [SwaggerOperation(Summary = "Criar moto", Description = "Cria uma nova moto (placa única).")]
    [SwaggerResponse(201, "Criado (Location aponta para o recurso)")]
    [SwaggerResponse(409, "Placa já existente")]
    [SwaggerResponse(400, "Dados inválidos")]
    [SwaggerRequestExample(typeof(MotoCreateDto), typeof(MotoCreateExample))]
    public async Task<IActionResult> Post([FromBody] Moto input)
    {
        if (await db.Motos.AnyAsync(x => x.Placa == input.Placa))
            return Conflict(new { error = "Placa já existente." });

        input.Id = Guid.NewGuid();
        input.CriadoEm = input.AtualizadoEm = DateTime.UtcNow;
        db.Motos.Add(input);
        await db.SaveChangesAsync();
        return Created($"/api/v1/motos/{input.Id}", new { input.Id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] Moto input)
    {
        var m = await db.Motos.FindAsync(id);
        if (m is null) return NotFound();

        m.Placa = input.Placa;
        m.Modelo = input.Modelo;
        m.Ano = input.Ano;
        m.Status = input.Status;
        m.Latitude = input.Latitude;
        m.Longitude = input.Longitude;
        m.AtualizadoEm = DateTime.UtcNow;

        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var m = await db.Motos.FindAsync(id);
        if (m is null) return NotFound();
        db.Motos.Remove(m);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
