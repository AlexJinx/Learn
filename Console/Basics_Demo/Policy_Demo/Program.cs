namespace Policy_Demo;
using Polly;

internal class Program
{
    static void Main(string[] args)
    {
        _ = Policy.Handle<Exception>().WaitAndRetryAsync(new[]
        {
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(1),
            TimeSpan.FromSeconds(1)
        }, (ex, count) =>
        {
            Console.WriteLine("文件下载异常", $"OSS内部异常，异常信息：{ex.Message}，重试次数：{count}");
        })
        .ExecuteAsync(async () =>
        {

            try
            {
                var a = 1;
                var b = 0;
                var c = a / b;
                await Task.Delay(1);
            }
            catch (Exception ex)
            {
                Console.WriteLine("文件下载异常", $"OSS内部异常，异常信息：{ex.Message}");
            }

        });

        Console.ReadKey();
    }
}