using Grpc.Core;
using Grpc.Net.Client;

using TestGrpcStream.Client;

Console.WriteLine("GRPC Stream 客户端");

Console.Write("请输入请求参数（比如 hello）：");

var method = Console.ReadLine();
if (string.IsNullOrEmpty(method))
{
    Console.WriteLine("请求参数错误");
}

using (var channel = GrpcChannel.ForAddress("https://192.168.13.60:7111"))
{
    var req = new HelloRequest
    {
        Method = method
    };

    var client = new MKStream.MKStreamClient(channel);
    using var call = client.HelloStream(req);

    await foreach (var res in call.ResponseStream.ReadAllAsync())
    {
        Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {res.Message}");
    }

    Console.WriteLine();
    Console.WriteLine("接收完毕");
}