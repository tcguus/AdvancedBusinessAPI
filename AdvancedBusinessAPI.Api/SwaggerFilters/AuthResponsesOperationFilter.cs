using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AdvancedBusinessAPI.Api.SwaggerFilters;

public class AuthResponsesOperationFilter : IOperationFilter
{
  public void Apply(OpenApiOperation op, OperationFilterContext ctx)
  {
    
    var isAnon = ctx.MethodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any()
                 || (ctx.MethodInfo.DeclaringType?.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any() ?? false);
    if (isAnon) return;

    op.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
    op.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });
  }
}
