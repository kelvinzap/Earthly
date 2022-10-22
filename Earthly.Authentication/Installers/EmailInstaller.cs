using Earthly.Authentication.Settings;

namespace Earthly.Authentication.Installers;

public class EmailInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.
        
        var emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>();
        services.AddSingleton(emailSettings);
    }
}