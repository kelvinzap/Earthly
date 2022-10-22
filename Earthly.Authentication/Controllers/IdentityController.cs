using System.Security.Claims;
using Earthly.Authentication.Contracts.V1;
using Earthly.Authentication.Contracts.V1.Request;
using Earthly.Authentication.Contracts.V1.Response;
using Earthly.Authentication.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Earthly.Authentication.Controllers;

[ApiController]
public class IdentityController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IAuthEmailService _emailService;
    public IdentityController(IAuthService authService, IAuthEmailService emailService)
    {
        _authService = authService;
        _emailService = emailService;
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
        
        //generation of the email confirm link
        var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
        var link = baseUrl + "/" + ApiRoutes.Identity.ConfirmEmail + "?userId=" + authResponse.UserId + "&code=" + authResponse.EmailConfirmCode;

        await _emailService.SendAsync(authResponse.Email, "Confirm Your Earthly Account", link, true);
        
        return Ok(new AuthSuccessResponse
        {
            ApiKey = authResponse.ApiKey
        });
    }

    [HttpGet(ApiRoutes.Identity.ConfirmEmail)]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, string code)
    {
        var result = await _authService.ConfirmEmailAsync(userId, code);

        return result ? Ok("Confirmed") : BadRequest("Something went wrong");
    }
    public override SignInResult SignIn(ClaimsPrincipal principal, AuthenticationProperties properties, string authenticationScheme)
    {
        return base.SignIn(principal, properties, authenticationScheme);
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

    [HttpGet(ApiRoutes.Identity.Verify)]
    public async Task<IActionResult> Verify([FromHeader] string apiKey)
    {
        var authResponse = await _authService.VerifyApiKeyAsync(apiKey);

        return authResponse ? Ok() : BadRequest();
    }

}