using Earthly.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Earthly.Installers;

public class DbInstaller : IInstaller
{
    public void InstallServices(IConfiguration configuration, IServiceCollection services)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(connectionString));
        
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<DataContext>();
    }
}