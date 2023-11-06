using Hangfire;

namespace Hangfire_Demo;

public class StockJob
{

    [AutomaticRetry(Attempts = 2, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    public void AutoUpdateSourcePriceAndRefreshCurrentEarningsAsync()
    {
        Console.WriteLine($"Hangfire-{DateTime.Now:HH:mm:ss}");
    }
}
