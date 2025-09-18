using Swashbuckle.AspNetCore.Filters;
using AdvancedBusinessAPI.Application.DTOs;

namespace AdvancedBusinessAPI.Api.SwaggerExamples;

public class MotoCreateExample : IExamplesProvider<MotoCreateDto>
{
  public MotoCreateDto GetExamples() => new()
  {
    Placa = "XYZ1A23",
    Modelo = "CG 160",
    Ano = 2023,
    Status = 0,
    Latitude = -23.55,
    Longitude = -46.63
  };
}

public class MotosListResponseExample : IExamplesProvider<object>
{
  public object GetExamples() => new
  {
    items = new[]
    {
      new {
        id = "6f6f2e8f-f6f7-4f8e-8c2d-21a1b1a1a1a1",
        placa = "ABC1D23",
        modelo = "NMax 160",
        ano = 2023,
        status = 0,
        links = new[] {
          new { rel="self", href="/api/v1/motos/6f6f2e8f-f6f7-4f8e-8c2d-21a1b1a1a1a1", method="GET" },
          new { rel="update", href="/api/v1/motos/6f6f2e8f-f6f7-4f8e-8c2d-21a1b1a1a1a1", method="PUT" },
          new { rel="delete", href="/api/v1/motos/6f6f2e8f-f6f7-4f8e-8c2d-21a1b1a1a1a1", method="DELETE" }
        }
      }
    },
    links = new[]
    {
      new { rel="self", href="/api/v1/motos?page=1&pageSize=20", method="GET" },
      new { rel="next", href="/api/v1/motos?page=2&pageSize=20", method="GET" },
      new { rel="last", href="/api/v1/motos?page=5&pageSize=20", method="GET" }
    }
  };
}
