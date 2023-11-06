using Grpc.Core;

namespace TestGrpcStream.Server.Services;

public class MKStreamService : MKStream.MKStreamBase
{
    public override async Task HelloStream(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
    {
        var result = new HelloReply();

        if (request == null || string.IsNullOrEmpty(request.Method))
        {
            result.Message = "非法参数";
            await responseStream.WriteAsync(result);
            return;
        }

        Console.WriteLine($"客户端请求参数：{request.Method}");

        if (!request.Method.Equals("hello", StringComparison.OrdinalIgnoreCase))
        {
            result.Message = "请输入 hello 然后回车";
            await responseStream.WriteAsync(result);
            return;
        }

        var max = 10;
        result.Message = $"即将开始发送 {max} 条数据：";
        await responseStream.WriteAsync(result);

        for (var i = 1; i <= max; i++)
        {
            result.Message = $"第 {i} 条";
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} 发送 {result.Message}");

            await responseStream.WriteAsync(result);

            Thread.Sleep(300);
        }

        result.Message = $"{max} 条数据全部发送完毕，See you";
        await responseStream.WriteAsync(result);
    }
}
