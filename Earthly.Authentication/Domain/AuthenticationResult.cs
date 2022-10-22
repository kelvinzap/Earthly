namespace Earthly.Authentication.Domain;

public class AuthenticationResult
{
    public string ApiKey { get; set; }
    public bool Success { get; set; }
    public IEnumerable<string> Errors { get; set; }
    public string EmailConfirmCode { get; set; }
    public string UserId { get; set; }
    public string Email { get; set; }
}