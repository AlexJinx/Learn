using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWT.Demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {

        [HttpGet("Get"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public string GetStr()
        {
            return "Test";
        }


        [HttpGet("GetToken")]
        public IActionResult GetToken()
        {
            return Ok(new { Token = BuildToken("admin") });
        }


        private string BuildToken(string userId)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("bfe9a84567aa424da5be0beedcd3920d");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = "Issuer",
                    Audience = "Audience",
                    Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userId), new Claim("Extend", "1111") }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


        }
    }
}
