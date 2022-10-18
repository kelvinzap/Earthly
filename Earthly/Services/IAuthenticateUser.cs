namespace Earthly.Services;

public interface IAuthenticateUser
{
    Task<bool> VerifyApikey(string apiKey);
}