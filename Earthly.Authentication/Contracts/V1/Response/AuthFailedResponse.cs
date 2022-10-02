namespace Earthly.Authentication.Contracts.V1.Response;

public class AuthFailedResponse
{
    public IEnumerable<string> Errors { get; set; }
}