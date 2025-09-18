namespace AdvancedBusinessAPI.Application.DTOs;

public record RegisterDto(string Nome, string Email, string Senha);
public record LoginDto(string Email, string Senha);
