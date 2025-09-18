using AdvancedBusinessAPI.Application.DTOs;
using FluentValidation;

namespace AdvancedBusinessAPI.Application.Validation;

public class MotoCreateValidator : AbstractValidator<MotoCreateDto>
{
  public MotoCreateValidator()
  {
    RuleFor(x => x.Placa).NotEmpty().Length(7, 8);
    RuleFor(x => x.Modelo).NotEmpty().MaximumLength(60);
    RuleFor(x => x.Ano).InclusiveBetween(2000, DateTime.UtcNow.Year + 1);
  }
}
