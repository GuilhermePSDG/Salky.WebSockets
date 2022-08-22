using Example;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Salky.WebSockets.Fluent;
using Salky.WebSockets.Router.Extensions;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSalkyWebSocket(options =>
{
    options.SetAuthGuard<ConnectionAuthGuard>();
    options.UseDefaultConnectionMannager((conMannager) => conMannager.UseBasicConnectionRemotion());
    options.UseRouter();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSalkyWebSocket();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();

public static class Config
{
    public static string JwtKey = "aoskdkaosdkokoqwodkoqkwodkoqwkodkqkowdojkofjwioejfowej";
}