using Earthly.Authentication.Data;
using Earthly.Authentication.Installers;
using Earthly.Authentication.Services;
using Earthly.Authentication.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.InstallServicesInAssembly(builder.Configuration);
var encryptionKey = builder.Configuration.GetSection("EncryptionKey").Value;
builder.Services.AddSingleton(encryptionKey);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();