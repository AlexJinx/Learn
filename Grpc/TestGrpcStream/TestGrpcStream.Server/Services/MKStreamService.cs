using Grpc.Core;

namespace TestGrpcStream.Server.Services;

public class MKStreamService : MKStream.MKStreamBase
{
    public override async Task HelloStream(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
    {
        var result = new HelloReply();

        if (request == null || string.IsNullOrEmpty(request.Method))
        {
            result.Message = "�Ƿ�����";
            await responseStream.WriteAsync(result);
            return;
        }

        Console.WriteLine($"�ͻ������������{request.Method}");

        if (!request.Method.Equals("hello", StringComparison.OrdinalIgnoreCase))
        {
            result.Message = "������ hello Ȼ��س�";
            await responseStream.WriteAsync(result);
            return;
        }

        var max = 10;
        result.Message = $"������ʼ���� {max} �����ݣ�";
        await responseStream.WriteAsync(result);

        for (var i = 1; i <= max; i++)
        {
            result.Message = $"�� {i} ��";
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} ���� {result.Message}");

            await responseStream.WriteAsync(result);

            Thread.Sleep(300);
        }

        result.Message = $"{max} ������ȫ��������ϣ�See you";
        await responseStream.WriteAsync(result);
    }
}
