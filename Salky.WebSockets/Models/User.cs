// configure
using Salky.WebSockets.Contracts;
using System.Security.Claims;

namespace Salky.WebSockets.Models;

public class WebSocketUser
{
    public WebSocketUser(Key userId, List<Claim> claims)
    {
        UserId = userId;
        Claims = claims;
    }
    public Key UserId { get; private set; }
    public List<Claim> Claims { get; private set; }

    public void Clear()
    {
        Claims.Clear();
        Claims = null;
        UserId = "";
    }

}