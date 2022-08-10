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

#region Use with own authentication class
builder.Services.AddSalkyWebSocket(options =>
{
    options.SetAuthGuard<ConnectionAuthGuard>();
    options.MapRoutes();
});
#endregion

#region Use With Microsoft.AspNetCore.Authentication.JwtBearer
//builder.Services.AddSalkyWebSocket(options =>
//{
//    options.MapRoutes();
//    options.UseAspNetAuth(x => x.First(x => x.Type == ClaimTypes.NameIdentifier));
//});
//builder.Services
//    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuerSigningKey = true,
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.JwtKey)),
//            ValidateIssuer = false,
//            ValidateAudience = false,
//            ValidateLifetime = true,
//        };
//        options.UseSalkWebSocketsAspNetAuthTokenParser();
//    });

//Js Code
//const ws = new WebSocket("wss://localhost:7075",["Identifier", "TOKEN_HERE"]);
//
#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
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