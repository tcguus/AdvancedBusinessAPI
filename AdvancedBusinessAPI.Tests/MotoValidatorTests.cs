using AdvancedBusinessAPI.Application.DTOs;
using AdvancedBusinessAPI.Application.Validation;
using FluentAssertions;

namespace AdvancedBusinessAPI.Tests;

public class MotoValidatorTests
{
  [Fact]
  public void MotoCreateDto_DeveSerValido()
  {
    var dto = new MotoCreateDto { Placa="XYZ1A23", Modelo="CG 160", Ano=2024 };
    var validator = new MotoCreateValidator();
    validator.Validate(dto).IsValid.Should().BeTrue();
  }

  [Fact]
  public void MotoCreateDto_ComPlacaVazia_DeveFalhar()
  {
    var dto = new MotoCreateDto { Placa="", Modelo="CG 160", Ano=2024 };
    var validator = new MotoCreateValidator();
    validator.Validate(dto).IsValid.Should().BeFalse();
  }
}
