using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using SignalR_Service_Demo.Models.SysModels;

namespace SignalR_Service_Demo.Codes;


[Authorize]
[ApiController]
[Route("api/[controller]")]
public abstract class ApiBase : ControllerBase
{
    protected MKSiteModel MKSite { get; private set; }

    public ApiBase(IOptions<MKSiteModel> mkSite)
    {
        MKSite = mkSite.Value;
    }
}