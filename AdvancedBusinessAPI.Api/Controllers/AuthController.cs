using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AdvancedBusinessAPI.Domain;
using AdvancedBusinessAPI.Infrastructure;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using AdvancedBusinessAPI.Api.SwaggerExamples;
using AdvancedBusinessAPI.Application.DTOs;




namespace AdvancedBusinessAPI.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[SwaggerTag("Autenticação (cadastro, login e identificação do usuário).")]
public class AuthController(AppDbContext db, IConfiguration cfg) : ControllerBase
{
  
  
    public record RegisterDto(string Nome, string Email, string Senha);
    public record LoginDto(string Email, string Senha);
    
    /// <summary>Login</summary>
    /// <remarks>Retorna um JWT válido por 8 horas.</remarks>
    [HttpPost("register")]
    [SwaggerOperation(Summary = "Cadastrar usuário", Description = "Cria um novo usuário no sistema.")]
    [SwaggerResponse(201, "Usuário criado")]
    [SwaggerResponse(409, "Email já cadastrado")]
    [SwaggerResponse(400, "Dados inválidos")]
    [SwaggerRequestExample(typeof(RegisterDto), typeof(RegisterRequestExample))]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (await db.Usuarios.AnyAsync(u => u.Email == dto.Email))
            return Conflict(new { error = "Email já cadastrado." });

        var u = new Usuario {
            Id = Guid.NewGuid(),
            Nome = dto.Nome.Trim(),
            Email = dto.Email.Trim().ToLower(),
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha)
        };
        db.Usuarios.Add(u);
        await db.SaveChangesAsync();
        return Created($"/api/v1/usuarios/{u.Id}", new { u.Id, u.Nome, u.Email });
    }
    
    /// <summary>Login</summary>
    /// <remarks>Retorna um JWT válido por 8 horas.</remarks>
    [HttpPost("login")]
    [SwaggerOperation(Summary = "Login", Description = "Autentica e retorna token JWT.")]
    [SwaggerResponse(200, "Token gerado")]
    [SwaggerResponse(401, "Credenciais inválidas")]
    [SwaggerRequestExample(typeof(LoginDto), typeof(LoginRequestExample))]
    [SwaggerResponseExample(200, typeof(LoginResponseExample))]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var u = await db.Usuarios.FirstOrDefaultAsync(x => x.Email == dto.Email.ToLower());
        if (u is null || !BCrypt.Net.BCrypt.Verify(dto.Senha, u.SenhaHash))
            return Unauthorized(new { error = "Credenciais inválidas." });

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, u.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, u.Email),
            new Claim("name", u.Nome)
        };
        var token = new JwtSecurityToken(
            issuer: cfg["Jwt:Issuer"],
            audience: cfg["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return Ok(new { token = jwt });
    }

    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        // opcional: requer [Authorize] + extrair userId do token
        return Ok(new { ok = true });
    }
}
