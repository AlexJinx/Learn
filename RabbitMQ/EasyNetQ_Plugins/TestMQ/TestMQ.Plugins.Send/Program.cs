using EasyNetQ;
using EasyNetQ.Topology;

using MK.Common;

using System;

namespace TestMQ.Plugins.Send
{
    public class UserInfo
    {
        public int Delay { get; set; }

        public string Name { get; set; }

        public long At { get; set; }
    }

    class Program
    {
        //private const string _strDefault = "host=192.168.31.20:13817;username=dev;password=123456";
        //private const string _str = "host=192.168.31.20:13817;virtualhost=devHost;username=dev;password=123456";
        private const string _strDefault = "host=101.34.61.66:5672;username=jing;password=123456";
        private const string _str = "host=101.34.61.66:5672;virtualhost=/vjing;username=jing;password=123456";

        static void Main(string[] args)
        {
            Console.WriteLine("生产者");

            //EasyNetQDelay();
            EasyNetQDelay2();
        }

        #region EasyNetQDelay
        static void EasyNetQDelay()
        {
            Console.WriteLine("等待发送内容");

            using var bus = RabbitHutch.CreateBus(_strDefault, reg => reg.EnableDelayedExchangeScheduler());

            string msg;

            while ((msg = Console.ReadLine()) != "exit")
            {
                var user = new UserInfo
                {
                    Delay = new Random().Next(1000, 5000),
                    Name = msg,
                    At = DateTimeOffset.Now.ToUnixTimeMilliseconds()
                }.ToJson();

                bus.Scheduler.FuturePublish(user, typeof(string), TimeSpan.FromSeconds(3));
                Console.WriteLine($"{DateTimeOffset.Now.ToUnixTimeMilliseconds()} 发送成功 {user}");
            }
        }
        #endregion

        #region EasyNetQDelay2（推荐）
        static void EasyNetQDelay2()
        {
            Console.WriteLine("等待发送内容");

            using var bus = RabbitHutch.CreateBus(_str);

            // 创建延时 exchange，并和普通 exchange 绑定，数据发送到延时 exchange
            var exDelay = bus.Advanced.ExchangeDeclare("jack.ex.delay", cfg => cfg.AsDelayedExchange(ExchangeType.Direct));
            var exNormal = bus.Advanced.ExchangeDeclare("jack.ex.normal", ExchangeType.Direct);
            bus.Advanced.Bind(exDelay, exNormal, "delay");

            // 创建普通队列，并和普通 exchange 绑定
            var qNormal = bus.Advanced.QueueDeclare("jack.q.normal");
            bus.Advanced.Bind(exNormal, qNormal, "delay");

            string msg;

            while ((msg = Console.ReadLine()) != "exit")
            {
                var user = new UserInfo
                {
                    Delay = new Random().Next(6, 10) * 1000,
                    Name = msg,
                    At = DateTimeOffset.Now.ToUnixTimeMilliseconds()
                };

                // 针对单个消息，设置延迟时间（毫秒）
                var msgProp = new MessageProperties();
                msgProp.Headers.Add("x-delay", user.Delay);

                bus.Advanced.Publish(exDelay, "delay", false, new Message<string>(user.ToJson(), msgProp));
                Console.WriteLine($"{DateTimeOffset.Now.ToUnixTimeMilliseconds()} 发送成功 {user.ToJson()}");
            }
        }
        #endregion
    }
}
