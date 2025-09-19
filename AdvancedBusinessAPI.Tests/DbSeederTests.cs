using AdvancedBusinessAPI.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace AdvancedBusinessAPI.Tests;

public class DbSeederTests
{
  [Fact]
  public async Task Seed_DeveCriarUsuarioEMotos()
  {
    var options = new DbContextOptionsBuilder<AppDbContext>()
      .UseInMemoryDatabase("seed-tests")
      .Options;

    await using var db = new AppDbContext(options);
    await DbSeeder.SeedAsync(db);

    db.Usuarios.Count().Should().BeGreaterThan(0);
    db.Motos.Count().Should().BeGreaterThan(0);
  }
}
