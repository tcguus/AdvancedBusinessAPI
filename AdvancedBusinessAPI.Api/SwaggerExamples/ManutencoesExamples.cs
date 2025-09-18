using Swashbuckle.AspNetCore.Filters;
using AdvancedBusinessAPI.Application.DTOs;

namespace AdvancedBusinessAPI.Api.SwaggerExamples;

public class ManutencaoCreateExample : IExamplesProvider<ManutencaoCreateDto>
{
  public ManutencaoCreateDto GetExamples() => new()
  {
    MotoId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
    Data = DateTime.UtcNow,
    Tipo = "Revisao",
    Descricao = "Troca de óleo e inspeção",
    Status = 0,
    Custo = 150.0m
  };
}
