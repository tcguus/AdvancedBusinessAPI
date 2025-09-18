using Swashbuckle.AspNetCore.Filters;
using AdvancedBusinessAPI.Application.DTOs;

namespace AdvancedBusinessAPI.Api.SwaggerExamples;

public class LoginRequestExample : IExamplesProvider<LoginDto>
{
  public LoginDto GetExamples() => new("teste@demo.com", "123456");
}

public class RegisterRequestExample : IExamplesProvider<RegisterDto>
{
  public RegisterDto GetExamples() => new("Usuário Teste", "teste@demo.com", "123456");
}

public class LoginResponseExample : IExamplesProvider<object>
{
  public object GetExamples() => new { token = "eyJhbGciOi..." };
}
