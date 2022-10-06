using Earthly.Authentication.Contracts.V1;
using Earthly.Authentication.Contracts.V1.Request;
using Microsoft.AspNetCore.Mvc;

namespace Earthly.Authentication.Controllers;

[ApiController]
public class IdentityController : ControllerBase
{
    [HttpPost(ApiRoutes.Identity.Register)]
    public async Task<IActionResult> Register([FromBody] CreateUserRequest request)
    {
        return Ok();
    }
    
}