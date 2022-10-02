namespace Earthly.Authentication.Contracts.V1.Request;

public class CreateNewApiKey
{
    public string ApiKey { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}