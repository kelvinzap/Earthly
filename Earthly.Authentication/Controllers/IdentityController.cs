using Earthly.Authentication.Contracts.V1;
using Earthly.Authentication.Contracts.V1.Request;
using Earthly.Authentication.Contracts.V1.Response;
using Earthly.Authentication.Services;
using Microsoft.AspNetCore.Mvc;

namespace Earthly.Authentication.Controllers;

[ApiController]
public class IdentityController : ControllerBase
{
    private readonly IAuthService _authService;

    public IdentityController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost(ApiRoutes.Identity.Register)]
    public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
    {
        var authResponse = await _authService.RegisterAsync(request);

        if (!authResponse.Success)
            return BadRequest(new AuthFailedResponse
            {
                Errors = authResponse.Errors
            });
        
        return Ok(new AuthSuccessResponse
        {
            ApiKey = authResponse.ApiKey
        });
    }
    
    [HttpPost(ApiRoutes.Identity.Login)]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
    {
        var authResponse = await _authService.LoginAsync(request);

        if (!authResponse.Success)
            return BadRequest(new AuthFailedResponse
            {
                Errors = authResponse.Errors
            });
        
        return Ok(new AuthSuccessResponse
        {
            ApiKey = authResponse.ApiKey
        });
    }

    [HttpPost(ApiRoutes.Identity.Regenerate)]
    public async Task<IActionResult> Regenerate([FromBody] CreateNewApiKey request)
    {
        var authResponse = await _authService.RegenerateAsync(request);

        if (!authResponse.Success)
            return BadRequest(new AuthFailedResponse
            {
                Errors = authResponse.Errors
            });

        return Ok(new AuthSuccessResponse
        {
            ApiKey = authResponse.ApiKey
        });
    }

}