using Marc2.Alert;
using Marc2.Extensions;
using Marc2.Services.Abstractions;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrelOptions(builder.Configuration, builder.Environment);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureCors();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureExceptionMiddleware();
builder.Services.ConfigureTokenService();
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureCertificateManagement();
builder.Services.ConfigureAlertService();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionMiddleware();

app.UseHttpsRedirection();

app.UseCors("DefaultPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

