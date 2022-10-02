namespace Earthly.Authentication.Domain;

public class AuthenticationResult
{
    public string ApiKey { get; set; }
    public bool Success { get; set; }
    public IEnumerable<string> Errors { get; set; }
}