using System.ComponentModel.DataAnnotations;

namespace Earthly.Authentication.Contracts.V1.Request;

public class UserLoginRequest
{
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set; }
}