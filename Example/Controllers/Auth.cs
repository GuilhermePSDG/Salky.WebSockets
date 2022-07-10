using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Example.Controllers
{
    [ApiController()]
    [Route("auth")]
    public class Auth : ControllerBase
    {
        [HttpGet("token/{userId}")]
        public IActionResult Token(string userId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var Claims = new List<Claim>()
            {
                new Claim(System.Security.Claims.ClaimTypes.NameIdentifier,userId)
            };
            var expireDate = DateTime.UtcNow.Add(TimeSpan.FromDays(30));
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(Claims),
                Expires = expireDate,
                SigningCredentials = creds,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);
            return Ok(tokenHandler.WriteToken(token));
        }
    }
}
