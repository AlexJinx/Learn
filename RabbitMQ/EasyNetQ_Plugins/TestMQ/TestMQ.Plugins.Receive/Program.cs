using EasyNetQ;

using MK.Common;

using System;

namespace TestMQ.Plugins.Receive
{
    public class UserInfo
    {
        public int Delay { get; set; }

        public string Name { get; set; }

        public long At { get; set; }
    }

    class Program
    {
        private const string _strDefault = "host=101.34.61.66:5672;username=jing;password=123456";
        private const string _str = "host=101.34.61.66:5672;virtualhost=/vjing;username=jing;password=123456";

        static void Main(string[] args)
        {
            Console.WriteLine("消费者");

            //EasyNetQDelay();
            EasyNetQDelay2();
        }

        #region EasyNetQDelay
        static void EasyNetQDelay()
        {
            using var bus = RabbitHutch.CreateBus(_strDefault);
            bus.PubSub.Subscribe<string>(string.Empty, EasyNetQHandle);

            Console.WriteLine("等待中……（回车退出）");
            Console.ReadLine();
        }

        static void EasyNetQHandle(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTimeOffset.Now.ToUnixTimeMilliseconds()} 收到消息：{msg}");
            Console.ResetColor();

            var user = msg.ToJson<UserInfo>();
        }
        #endregion

        #region EasyNetQDelay2（推荐）
        static void EasyNetQDelay2()
        {
            using var bus = RabbitHutch.CreateBus(_str);
            var qNormal = bus.Advanced.QueueDeclare("jack.q.normal");
            bus.Advanced.Consume<string>(qNormal, EasyNetQHandle2);

            Console.WriteLine("等待中……（回车退出）");
            Console.ReadLine();
        }

        static void EasyNetQHandle2(IMessage<string> msg, MessageReceivedInfo msgInfo)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTimeOffset.Now.ToUnixTimeMilliseconds()} 收到消息：{msg.Body}");
            Console.ResetColor();

            var user = msg.Body.ToJson<UserInfo>();
        }
        #endregion
    }
}
