using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using Microsoft.Extensions.Configuration;

namespace Server.Controlles
{
    [Route("/")]
    public class LoginController : Controller
    {
        private const int DEFAULT_LIFE_TIME = 10;
        private readonly IConfiguration _configuration;
        public LoginController(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        [HttpPost("/token")]
        public IActionResult Token([FromBody] TokenModel token)
        {
            if(token == null)
            {
                return BadRequest();
            }

            var now = DateTime.UtcNow;
            var expiresTime = GetExpriresTime(now);
            var claims = new List<Claim>()
            {
                new Claim("mussic_auth", token.Token)
            };

            var jwtToken = GetJwtToken(now, expiresTime, claims);

            var response = new
            {
                access_token = jwtToken
            };
            return Json(response);
        }

        private DateTime GetExpriresTime(DateTime now) {
            var lifeTimeString = _configuration.GetSection("JWT:LifetimeMinutes").Value;
            var time = DEFAULT_LIFE_TIME;
            if (int.TryParse(lifeTimeString, out var value))
            {
                time = value;
            }

            return now.Add(TimeSpan.FromMinutes(time));
        }

        private string GetJwtToken(DateTime now, DateTime expiresTime, IEnumerable<Claim> claims)
        {
            var jwt = new JwtSecurityToken(
                issuer: _configuration.GetSection("JWT:Issuer").Value,
                audience: _configuration.GetSection("JWT:Audience").Value,
                notBefore: now,
                claims: claims,
                expires: expiresTime,
                signingCredentials: new SigningCredentials(
                    AuthOptions.GetSymmetricSecurityKey(
                        _configuration.GetSection("JWT:Secret").Value),
                        SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
    }
}
