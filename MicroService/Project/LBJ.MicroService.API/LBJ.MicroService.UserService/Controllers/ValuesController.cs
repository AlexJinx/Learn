using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBJ.MicroService.UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var ip = HttpContext.Connection.LocalIpAddress.MapToIPv4().ToString();
            var port = HttpContext.Connection.LocalPort.ToString();

            return await Task.FromResult(Ok($"{ip}:{port}"));
        }
    }
}
