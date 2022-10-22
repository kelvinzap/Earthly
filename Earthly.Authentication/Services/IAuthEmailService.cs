namespace Earthly.Authentication.Services;

public interface IAuthEmailService
{
    Task SendAsync(string mailTo, string subject, string message, bool isHtml = false);
}