using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using MK.Common;

using Morin.API.Business;
using Morin.API.Code;
using Morin.API.Entity;
using Morin.API.Models;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace Morin.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : APIBase
{
    private readonly MorinConfigModel _mrCfg;

    private readonly HtmlEncoder _htmlEncoder;

    private readonly UserBusiness _userBiz;
    private readonly UserBusiness _userBizW;
    public UserController(HtmlEncoder htmlEncoder, IOptions<MorinConfigModel> mrCfg)
    {
        _mrCfg = mrCfg.Value;
        _htmlEncoder = htmlEncoder;
        _userBiz = new UserBusiness(mrCfg.Value.MySQLConn);
        _userBizW = new UserBusiness(mrCfg.Value.MySQLConnWrite);
    }


    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<MKResult<UserTokenModel>> LoginAsync([FromBody] UserLoginModel model)
    {
        #region 基本验证
        if (model == null)
        {
            return new MKResult<UserTokenModel>(code: 400, msg: "非法参数");
        }

        if (string.IsNullOrWhiteSpace(model.Account))
        {
            return new MKResult<UserTokenModel>(code: 400, msg: "账户名为空");
        }

        if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Length != 32)
        {
            return new MKResult<UserTokenModel>(code: 400, msg: "登录密码为空");
        }
        #endregion

        #region 数据库验证
        var info = await _userBiz.GetUserCountByAccount(_htmlEncoder.Encode(model.Account.Trim()));
        if (info.Code != 200)
        {
            return new MKResult<UserTokenModel>(code: info.Code, msg: info.Msg);
        }

        if (info.TotalCount == 0)
        {
            return new MKResult<UserTokenModel>(code: 400, msg: "您还没有注册");
        }

        var newPass = string.Concat(model.Password, info.Body.PasswordSalt).MD5();
        if (!newPass.Equals(info.Body.Password, StringComparison.OrdinalIgnoreCase))
        {
            return new MKResult<UserTokenModel>(code: 400, msg: "登录名称或密码错误");
        }
        #endregion

        InitAPI();
        await _userBiz.InsertLoginLogAsync(MKHelper.GetULongID(), info.Body.Id, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), CurrentUser.UserAgent, CurrentUser.RemoteIP);

        var basic = new UserInfo
        {
            Id = info.Body.Id,
            UserName = info.Body.UserName,
            Account = info.Body.Account,
        };

        return GetToken(basic);
    }

    [HttpPost("reg")]
    [AllowAnonymous]
    public async Task<MKResult> RegAsync(RegInModel model)
    {
        if (model is null)
        {
            return new MKResult(400, "非法参数");
        }

        if (string.IsNullOrWhiteSpace(model.RePassword) ||
            string.IsNullOrWhiteSpace(model.UserName) ||
            string.IsNullOrWhiteSpace(model.Password) ||
            string.IsNullOrWhiteSpace(model.Account) ||
            string.IsNullOrWhiteSpace(model.Birthday))
        {
            return new MKResult(400, "非法参数");
        }

        if (Regex.IsMatch(model.Account, @"^1(3|4|5|6|7|8|9)d{9}$"))
        {
            return new MKResult(400, "请输入正确的手机号");
        }

        var count = await _userBiz.GetUserCountByAccount(model.Account);
        if (count.TotalCount > 0)
        {
            return new MKResult(400, "手机号已被注册");
        }

        if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Length != 32)
        {
            return new MKResult(400, "登录密码为空");
        }

        if (!model.Password.Equals(model.RePassword, StringComparison.OrdinalIgnoreCase))
        {
            return new MKResult(400, "两次输入的密码不一致");
        }

        InitAPI();
        var salt = string.Concat(MKHelper.GetGuid(), MKHelper.GetGuid());
        var info = new UserInfo
        {
            Id = MKHelper.GetULongID(),
            Account = model.Account,
            UserName = _htmlEncoder.Encode(model.UserName),
            Gender = model.Gender,
            Birthday = model.Birthday,
            PasswordSalt = salt,
            Password = string.Concat(model.Password, salt).MD5(),
            AddedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            AddedIP = CurrentUser.RemoteIP,
            AddedAgent = CurrentUser.UserAgent
        };

        return await _userBizW.InsertAsync(info);
    }

    [HttpPost("token/refresh")]
    [AllowAnonymous]
    public async Task<MKResult<UserTokenModel>> RefreshTokenAsync([FromBody] string token)
    {
        #region 基本验证
        if (string.IsNullOrWhiteSpace(token))
        {
            return new MKResult<UserTokenModel>(code: 400, msg: "非法参数");
        }

        ulong userId;

        try
        {
            var oldToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            userId = ulong.Parse(oldToken.Payload[JwtRegisteredClaimNames.Sub].ToString());
        }
        catch
        {
            return new MKResult<UserTokenModel>(code: 400, msg: "非法 token");
        }

        if (userId == 0)
        {
            return new MKResult<UserTokenModel>(code: 400, msg: "非法 token");
        }
        #endregion

        #region 数据库验证
        var info = await _userBiz.GetUserInfoAsync(userId);
        if (info.Code != 200)
        {
            return new MKResult<UserTokenModel>(code: info.Code, msg: info.Msg);
        }

        if (info.TotalCount == 0)
        {
            return new MKResult<UserTokenModel>(code: 400, msg: "禁止换取 token");
        }
        #endregion

        return GetToken(info.Body);
    }

    private MKResult<UserTokenModel> GetToken(UserInfo info)
    {
        var now = DateTimeOffset.Now;

        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, info.Id.ToString(), ClaimValueTypes.UInteger64),
                new Claim(JwtRegisteredClaimNames.Jti, MKHelper.GetGuid(), ClaimValueTypes.String),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            };

        var expiry = TimeSpan.FromMinutes(_mrCfg.JwtExpireMinutes);

        var signKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_mrCfg.JwtSecret));

        var jwt = new JwtSecurityToken(_mrCfg.JwtIss, _mrCfg.JwtAud, claims, now.DateTime, now.Add(expiry).DateTime, new SigningCredentials(signKey, SecurityAlgorithms.HmacSha256));

        var result = new UserTokenModel
        {
            UserId = info.Id,
            UserName = info.UserName,
            Account = info.Account,
            Token = new JwtSecurityTokenHandler().WriteToken(jwt),
            Expire = expiry.TotalSeconds.ToInt()
        };

        return new MKResult<UserTokenModel>(result, 1);
    }
}



