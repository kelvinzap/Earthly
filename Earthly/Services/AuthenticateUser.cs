using System.Net;
using System.Net.Http.Headers;

namespace Earthly.Services;

public class AuthenticateUser : IAuthenticateUser
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AuthenticateUser(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<bool> VerifyApikey(string apiKey)
    {
        var httpClient = _httpClientFactory.CreateClient("Authentication");

        //Made use of Earthly Authentication Server
        httpClient.DefaultRequestHeaders.Add("apiKey", apiKey);
        var requestContent = await httpClient.GetAsync("verify");
        
        if(requestContent.StatusCode == HttpStatusCode.OK) 
            return true;

        return false;
    }
}