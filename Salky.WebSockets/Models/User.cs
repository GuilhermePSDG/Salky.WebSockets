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
    public IStorage Storage { get; internal set; }

    public void Clear()
    {
        Claims.Clear();
        Storage.Clear();
        Storage = null;
        Claims = null;
        UserId = "";
    }

}