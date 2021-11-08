using EasyNetQ;

using MK.Common;

using System;

namespace TestMQ.Size.Receive
{
    public class UserInfo
    {
        public uint Id { get; set; }

        public string Name { get; set; }

        public long At { get; set; }
    }

    class Program
    {
        private const string _str = "host=192.168.31.20:13817;virtualhost=devHost;username=dev;password=123456";

        static void Main(string[] args)
        {
            Console.WriteLine("消费者");

            EasyNetQSizeFailure();
        }

        #region EasyNetQSizeFailure
        static void EasyNetQSizeFailure()
        {
            using var bus = RabbitHutch.CreateBus(_str);

            // 入队失败的数据
            var qSizeFailed = bus.Advanced.QueueDeclare("jack.q.size.failure", cfg => cfg.AsAutoDelete(true));
            bus.Advanced.Consume<string>(qSizeFailed, EasyNetQHandle);

            Console.WriteLine("等待中……（回车退出）");
            Console.ReadLine();
        }

        static void EasyNetQHandle(IMessage<string> msg, MessageReceivedInfo msgInfo)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTimeOffset.Now.ToUnixTimeMilliseconds()} 收到消息：{msg.Body}");
            Console.ResetColor();

            var user = msg.Body.ToJson<UserInfo>();
        }
        #endregion
    }
}
