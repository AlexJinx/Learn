using EasyNetQ;

using MK.Common;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System;
using System.Text;

using TestMQ.Entity;

namespace TestMQ.Receive
{
    class Program
    {
        private const string _str = "host=dev.zt:13817;virtualhost=devHost;username=dev;password=123456";

        static void Main(string[] args)
        {
            Console.WriteLine("消费者");

            //EasyNetQPubSub();
            //EasyNetQPubSubDelay();
            EasyNetQPubSubDelay2();
            //EasyNetQPubSubSize();
        }

        #region 原生写法
        static void OriginalMQ()
        {
            var factory = new ConnectionFactory
            {
                HostName = "dev.zt",
                Port = 13817,
                UserName = "dev",
                Password = "123456"
            };

            var conn = factory.CreateConnection();
            var chn = conn.CreateModel();
            var name = "jackQ";

            chn.QueueDeclare(name, true, false, false, null);

            var consumer = new EventingBasicConsumer(chn);
            consumer.Received += (model, ea) =>
            {
                Console.WriteLine($"{DateTimeOffset.Now.ToUnixTimeMilliseconds()} 收到消息：{Encoding.UTF8.GetString(ea.Body.ToArray())}");
            };

            chn.BasicConsume(name, true, consumer);

            Console.ReadLine();

            chn.Dispose();
            conn.Close();
        }
        #endregion

        #region EasyNetQ

        #region PubSub
        static void EasyNetQPubSub()
        {
            using var bus = RabbitHutch.CreateBus(_str);
            bus.PubSub.Subscribe<UserInfo>("JackReceive", EasyNetQHandle);
            Console.WriteLine("等待中……（回车退出）");
            Console.ReadLine();
        }
        #endregion

        #region PubSub Delay
        static void EasyNetQPubSubDelay()
        {
            using var bus = RabbitHutch.CreateBus(_str);
            bus.PubSub.Subscribe<UserInfo>(string.Empty, EasyNetQHandle);
            Console.WriteLine("等待中……（回车退出）");
            Console.ReadLine();
        }
        #endregion

        #region PubSub Delay2（推荐）
        static void EasyNetQPubSubDelay2()
        {
            using var bus = RabbitHutch.CreateBus(_str).Advanced;
            var qOrderCancel = bus.QueueDeclare("jack.q.order.cancel");
            bus.Consume<UserInfo2>(qOrderCancel, EasyNetQHandle2);
            Console.WriteLine("等待中……（回车退出）");
            Console.ReadLine();
        }
        #endregion

        #region PubSub Size
        static void EasyNetQPubSubSize()
        {
            using var bus = RabbitHutch.CreateBus(_str).Advanced;
            var qSize = bus.QueueDeclare("jack.q.size");
            bus.Consume<UserInfo2>(qSize, EasyNetQHandle2);
            Console.WriteLine("等待中……（回车退出）");
            Console.ReadLine();
        }
        #endregion

        static void EasyNetQHandle(UserInfo user)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTimeOffset.Now.ToUnixTimeMilliseconds()} 收到消息：{user.Name}");
            Console.ResetColor();
        }

        static void EasyNetQHandle2(IMessage<UserInfo2> msg, MessageReceivedInfo msgInfo)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTimeOffset.Now.ToUnixTimeMilliseconds()} 收到消息：{msg.Body.Name}");
            Console.ResetColor();
        }
        #endregion
    }
}
