using Earthly.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Earthly.Filter;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
{
    private readonly string ApiKeyHeaderName = "ApiKey";
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        //Before
        //First try to find the apiKey header in the request header
        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var potentialApiKey))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var authenticateUser = context.HttpContext.RequestServices.GetRequiredService<IAuthenticateUser>();
        var result = await authenticateUser.VerifyApikey(potentialApiKey);

        if (!result)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        
        await next();
        
        //after
    }
}