using AdvancedBusinessAPI.Application.DTOs;
using FluentValidation;

namespace AdvancedBusinessAPI.Application.Validation;

public class ManutencaoCreateValidator : AbstractValidator<ManutencaoCreateDto>
{
  public ManutencaoCreateValidator()
  {
    RuleFor(x => x.MotoId).NotEmpty();
    RuleFor(x => x.Tipo).NotEmpty().MaximumLength(40);
    RuleFor(x => x.Data).NotEmpty();
    RuleFor(x => x.Custo).GreaterThanOrEqualTo(0).When(x => x.Custo.HasValue);
  }
}
