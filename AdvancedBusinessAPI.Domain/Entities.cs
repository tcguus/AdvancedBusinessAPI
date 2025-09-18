using System;

namespace AdvancedBusinessAPI.Domain;

public enum StatusMoto { Disponivel, EmUso, Manutencao }
public enum StatusManutencao { Pendente, Confirmada, Concluida, Cancelada }

public class Moto
{
  public Guid Id { get; set; }
  public string Placa { get; set; } = default!; // única
  public string Modelo { get; set; } = default!;
  public int Ano { get; set; }
  public string? Chassi { get; set; }
  public StatusMoto Status { get; set; } = StatusMoto.Disponivel;
  public double? Latitude { get; set; }
  public double? Longitude { get; set; }
  public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
  public DateTime AtualizadoEm { get; set; } = DateTime.UtcNow;
}

public class Manutencao
{
  public Guid Id { get; set; }
  public Guid MotoId { get; set; }
  public DateTime Data { get; set; }
  public string Tipo { get; set; } = "Outro"; // Revisao, TrocaOleo, etc.
  public string? Descricao { get; set; }
  public StatusManutencao Status { get; set; } = StatusManutencao.Pendente;
  public decimal? Custo { get; set; }

  // navegação simples (opcional)
  public Moto? Moto { get; set; }
}

public class Usuario
{
  public Guid Id { get; set; }
  public string Nome { get; set; } = default!;
  public string Email { get; set; } = default!; // único
  public string SenhaHash { get; set; } = default!;
  // sem roles por enquanto
}
