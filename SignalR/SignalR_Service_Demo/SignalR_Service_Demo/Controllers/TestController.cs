using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using MK.Common;

using SignalR_Service_Demo.Codes;
using SignalR_Service_Demo.Models.SysModels;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace SignalR_Service_Demo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ApiBase
{
    public TestController(IOptions<MKSiteModel> mkSite)
        : base(mkSite)
    { }


    [HttpGet("token/verify")]
    public async Task<string> GetAuthAsync()
    {
        return await Task.FromResult("成功");
    }

    [HttpGet("token")]
    [AllowAnonymous]
    public async Task<string> GetTokenAsync()
    {
        var now = DateTimeOffset.Now;
        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, "123123213213", ClaimValueTypes.UInteger64),
                new Claim(JwtRegisteredClaimNames.Jti, MKHelper.GetGuid(), ClaimValueTypes.String),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            };

        var expiry = TimeSpan.FromMinutes(MKSite.JwtExpireMinutes);   // token 过期时间

        var rsa = RSA.Create();
        rsa.ImportPrivateKey(RSAKeyType.Pkcs1, MKSite.JwtPri);

        var jwt = new JwtSecurityToken(MKSite.JwtIss, MKSite.JwtAud, claims, now.DateTime, now.Add(expiry).DateTime,
                                       new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha512));

        return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(jwt));
    }

    [HttpGet]
    public async Task<string> GetMsgAsync()
    {
        return await Task.FromResult("Msg");
    }
}
