namespace AdvancedBusinessAPI.Application.DTOs;

public class ManutencaoCreateDto
{
  public Guid MotoId { get; set; }
  public DateTime Data { get; set; }
  public string Tipo { get; set; } = "Outro";
  public string? Descricao { get; set; }
  public int Status { get; set; } = 0;
  public decimal? Custo { get; set; }
}

public class ManutencaoUpdateDto : ManutencaoCreateDto { }
