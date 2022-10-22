using Earthly.Authentication.Contracts.V1.Request;
using Earthly.Authentication.Domain;
using Microsoft.AspNetCore.Identity;

namespace Earthly.Authentication.Services;

public interface IAuthService
{
    public Task<AuthenticationResult> RegisterAsync(CreateUserRequest request);
    public Task<AuthenticationResult> LoginAsync(UserLoginRequest request);
    public Task<AuthenticationResult> RegenerateAsync(CreateNewApiKey request);
    public Task<bool> VerifyApiKeyAsync(string apiKey);
    public Task<bool> ConfirmEmailAsync(string userId, string token);
} 