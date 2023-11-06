using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

using Morin.API.Models;

using System.Security.Claims;

namespace Morin.API.Code;

public class APIBase : ControllerBase
{
    protected MorinConfigModel MrCfg { get; private set; }

    public CurrentUserModel CurrentUser { get; private set; }

    public APIBase()
    { }

    public APIBase(MorinConfigModel mrCfg)
    {
        MrCfg = mrCfg;
    }

    protected void InitAPI()
    {
        Request.Headers.TryGetValue(HeaderNames.UserAgent, out var agent);
        Request.Headers.TryGetValue("X-Forwarded-For", out var ip);

        CurrentUser = new CurrentUserModel
        {
            UserAgent = StringValues.IsNullOrEmpty(agent) ? string.Empty : agent[0],
            RemoteIP = StringValues.IsNullOrEmpty(ip) ? HttpContext.Connection.RemoteIpAddress.ToString() : ip[0]
        };

        if (!User.Identity.IsAuthenticated)
        {
            return;
        }

        _ = ulong.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);
        CurrentUser.Id = userId;
    }
}

