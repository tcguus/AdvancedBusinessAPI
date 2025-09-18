namespace AdvancedBusinessAPI.Application.DTOs;

public class MotoCreateDto
{
  public string Placa { get; set; } = default!;
  public string Modelo { get; set; } = default!;
  public int Ano { get; set; }
  public int Status { get; set; } = 0; // enum como int
  public double? Latitude { get; set; }
  public double? Longitude { get; set; }
}

public class MotoUpdateDto : MotoCreateDto { }
