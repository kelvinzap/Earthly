namespace Earthly.Authentication.Contracts.V1.Request;

public class CreateUserRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string UserName { get; set; }
}