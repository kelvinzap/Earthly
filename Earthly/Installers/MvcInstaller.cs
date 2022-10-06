using Earthly.Client;
using Earthly.Contracts.TimezoneDBApiResponses;
using Earthly.Services;
using Microsoft.OpenApi.Models;

namespace Earthly.Installers;

public class MvcInstaller : IInstaller
{
    public void InstallServices(IConfiguration configuration, IServiceCollection services)
    {

        services.AddControllersWithViews().AddXmlDataContractSerializerFormatters();
        services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "TweetBook Api",
                Version = "v1"
            });
                
            x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Jwt Authorization header using the bearer scheme",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });
            x.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
        
        services.AddScoped<ICountryService, CountryService>();
        services.AddSingleton<ICountriesDataApiService, CountriesDataApiService>();
        services.AddSingleton<ICountryTimezoneApiService, TimeZoneDBApiClient>();
        var timezoneDBApiKey = configuration.GetSection("TimezoneDBApiKey").Value;
        services.AddSingleton(timezoneDBApiKey);
    }
}