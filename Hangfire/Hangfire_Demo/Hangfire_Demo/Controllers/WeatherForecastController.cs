using Hangfire;

using Microsoft.AspNetCore.Mvc;

namespace Hangfire_Demo.Controllers;
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    public WeatherForecastController()
    { }

    [HttpGet]
    public void Get()
    {
        RecurringJob.RemoveIfExists("Test");
        RecurringJob.AddOrUpdate<StockJob>("Test", job => job.AutoUpdateSourcePriceAndRefreshCurrentEarningsAsync(), Cron.Daily(18, 45), TimeZoneInfo.Local);
    }
}