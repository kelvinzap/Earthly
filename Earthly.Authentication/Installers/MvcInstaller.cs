using Earthly.Authentication.Services;

namespace Earthly.Authentication.Installers;

public class MvcInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddScoped<IAuthService, AuthService>(); 
        services.AddScoped<IAuthEmailService, AuthEmailService>();
    }
}