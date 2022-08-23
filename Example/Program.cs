using Example;
using Salky.WebSockets.Contracts;
using Salky.WebSockets.Fluent;
using Salky.WebSockets.Router.Extensions;
using System.Reflection;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

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
