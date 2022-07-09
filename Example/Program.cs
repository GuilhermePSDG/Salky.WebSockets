using Example;
using Salky.WebSockets.Fluent;
using Salky.WebSockets.Models;
using Salky.WebSockets.Router.Extensions;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSalkyWebSocket(options =>
{
    options.SetAuthGuard<ConnectionAuthGuard>();
    options.MapRoutes();
});

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

app.MapControllers();

app.Run();
