using AdvancedBusinessAPI.Domain;
using BCrypt.Net;

namespace AdvancedBusinessAPI.Infrastructure;

public static class DbSeeder
{
  public static async Task SeedAsync(AppDbContext db)
  {
    if (!db.Usuarios.Any())
    {
      db.Usuarios.Add(new Usuario {
        Id = Guid.NewGuid(),
        Nome = "Usuário Demo",
        Email = "demo@mottu.com",
        SenhaHash = BCrypt.Net.BCrypt.HashPassword("123456")
      });
    }

    if (!db.Motos.Any())
    {
      var motos = new[]
      {
        new Moto { Id=Guid.NewGuid(), Placa="ABC1D23", Modelo="CG 160", Ano=2022, Status=StatusMoto.Disponivel },
        new Moto { Id=Guid.NewGuid(), Placa="EFG4H56", Modelo="NMax 160", Ano=2023, Status=StatusMoto.Disponivel },
        new Moto { Id=Guid.NewGuid(), Placa="IJK7L89", Modelo="Pop 110i", Ano=2021, Status=StatusMoto.Manutencao }
      };
      db.Motos.AddRange(motos);

      db.Manutencoes.Add(new Manutencao {
        Id = Guid.NewGuid(),
        MotoId = motos[2].Id,
        Data = DateTime.UtcNow.Date,
        Tipo = "Revisao",
        Descricao = "Troca de óleo e inspeção",
        Status = StatusManutencao.Pendente,
        Custo = 120m
      });
    }

    await db.SaveChangesAsync();
  }
}
