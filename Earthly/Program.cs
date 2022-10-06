using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Earthly.Data;
using Earthly.Installers;
using Earthly.Options;
using Earthly.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthorization();
builder.Services.AddControllers();

builder.Services.InstallServicesInAssembly(builder.Configuration);
//builder.Services.GetAllCountries();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();



app.UseRouting();

var swaggerSettings = new SwaggerSettings();
builder.Configuration.GetSection(nameof(SwaggerSettings)).Bind(swaggerSettings);

app.UseSwagger(options =>
{
    options.RouteTemplate = swaggerSettings.JsonRoute;
});



app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(swaggerSettings.UIEndpoint, swaggerSettings.Description);
});


app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var serviceScope = app.Services.CreateScope())
{
    var countryManager = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
    var countriesNowApi = app.Services.GetRequiredService<ICountriesDataApiService>();
    var countriesTimezoneApi = app.Services.GetRequiredService<ICountryTimezoneApiService>();
    await countriesNowApi.GetAllCountriesAndData(countryManager);
    await countriesTimezoneApi.GetAllCountryTimeZones(countryManager);

}

app.Run();