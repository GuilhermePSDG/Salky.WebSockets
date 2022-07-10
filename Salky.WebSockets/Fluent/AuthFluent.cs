using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Salky.WebSockets.Fluent
{
    public static class AuthFluent
    {
        public static JwtBearerOptions UseSalkWebSocketsAspNetAuthTokenParser(this JwtBearerOptions jwtBearerOptions)
        {
            jwtBearerOptions.Events.OnMessageReceived = context =>
            {
                if (context.HttpContext.WebSockets.IsWebSocketRequest && context.Request.Headers.ContainsKey("sec-websocket-protocol"))
                {
                    var protocols = context.Request.Headers["sec-websocket-protocol"].ToString().Split(", ");
                    context.Request.Headers["sec-websocket-protocol"] = protocols[0];
                    context.Token = protocols[1];
                }
                return Task.CompletedTask;
            };
            return jwtBearerOptions;
        }
    }
}
