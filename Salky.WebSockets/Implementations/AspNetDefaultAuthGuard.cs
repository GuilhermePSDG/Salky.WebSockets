using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Salky.WebSockets.Contracts;
using Salky.WebSockets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Salky.WebSockets.Implementations
{
    public delegate Claim ClaimsIdSelector(List<Claim> claims);

    internal class AspNetDefaultAuthGuard : IConnectionAuthGuard
    {
        public ClaimsIdSelector ClaimsIdSelector { get; }

        public AspNetDefaultAuthGuard(ClaimsIdSelector claimsIdSelector)
        {
            ClaimsIdSelector = claimsIdSelector;
        }

        public async Task<WebSocketUser?> AuthenticateConnection(HttpContext http)
        {
            var identity = http.User.Identity;
            if (identity != null && identity.IsAuthenticated) 
                return this.Complete(http.User.Claims.ToList());
            else return await this.Authenticate(http);
        }

        private async Task<WebSocketUser?> Authenticate(HttpContext http)
        {
            var authResult = await http.AuthenticateAsync();
            if (authResult.Succeeded && authResult.Principal.Identity != null && authResult.Principal.Identity.IsAuthenticated)
            {
                return this.Complete(authResult.Principal.Claims.ToList());
            }
            else if (authResult.Failure != null)
            {
                ExceptionDispatchInfo.Capture(authResult.Failure).Throw();
            }
            else
            {
                await http.ChallengeAsync();
            }
            return null;
        }

        private WebSocketUser Complete(List<Claim> claims)
        {
            var selectedClaim = this.ClaimsIdSelector(claims);
            if (selectedClaim == null || string.IsNullOrEmpty(selectedClaim.Value)) throw new NullReferenceException(nameof(List<Claim>));
            return new WebSocketUser(selectedClaim.Value, claims);
        }
    }
}
