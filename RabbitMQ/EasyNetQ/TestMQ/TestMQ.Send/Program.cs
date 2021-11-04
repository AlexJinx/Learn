using EasyNetQ;

using MK.Common;

using RabbitMQ.Client;

using System;
using System.Collections.Generic;
using System.Text;

using TestMQ.Entity;

namespace TestMQ.Send
{
    class Program
    {
        private const string _str = "host=dev.zt:13817;virtualhost=devHost;username=dev;password=123456";

        static void Main(string[] args)
        {
            Console.WriteLine("生产者");

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
            string msg;

            do
            {
                Console.Write("发送内容：");
                msg = Console.ReadLine();
                var body = Encoding.UTF8.GetBytes(msg);
                chn.BasicPublish("", name, null, body);
                Console.WriteLine($"{DateTimeOffset.Now.ToUnixTimeMilliseconds()} 发送成功：{msg}");
            }
            while (msg.ToLower() != "exit");

            chn.Dispose();
            conn.Close();
        }
        #endregion

        #region EasyNetQ

        #region PubSub
        static void EasyNetQPubSub()
        {
            var user = new UserInfo
            {
                Id = 11,
                Name = "Jack11",
                Gender = Gender.Male,
                IsDeleted = true
            };

            Console.Write("发送内容：");
            using var bus = RabbitHutch.CreateBus(_str);
            while (Console.ReadLine().ToLower() != "exit")
            {
                bus.PubSub.Publish(user);
                Console.WriteLine($"{DateTimeOffset.Now.ToUnixTimeMilliseconds()} 发送成功");
            }
        }
        #endregion

        #region PubSub Delay
        static void EasyNetQPubSubDelay()
        {
            var user = new UserInfo
            {
                Id = 11,
                Name = "Jack11",
                Gender = Gender.Male,
                IsDeleted = true
            };

            Console.Write("发送内容：");
            using var bus = RabbitHutch.CreateBus(_str);
            while (Console.ReadLine().ToLower() != "exit")
            {
                bus.Scheduler.FuturePublish(user, user.GetType(), TimeSpan.FromSeconds(5));
                bus.Scheduler.FuturePublish(user, user.GetType(), TimeSpan.FromSeconds(10));
                Console.WriteLine($"{DateTimeOffset.Now.ToUnixTimeMilliseconds()} 发送成功");
            }
        }
        #endregion

        #region PubSub Delay2（推荐）
        static void EasyNetQPubSubDelay2()
        {
            var user = new Message<UserInfo2>(new UserInfo2
            {
                Id = 12,
                Name = "Jack12",
                Gender = Gender.Male,
                IsDeleted = true
            });

            Console.Write("发送内容：");
            using var bus = RabbitHutch.CreateBus(_str).Advanced;

            // 普通队列，通知消费者
            var exOrderCancel = bus.ExchangeDeclare("jack.ex.order.cancel", ExchangeType.Fanout);
            var qOrderCancel = bus.QueueDeclare("jack.q.order.cancel");
            bus.Bind(exOrderCancel, qOrderCancel, "orderCancel");

            // 死信队列，数据往此队列发
            var exOrder = bus.ExchangeDeclare("jack.ex.order", ExchangeType.Fanout);
            var qOrder = bus.QueueDeclare("jack.q.order", cfg => cfg.WithDeadLetterExchange(exOrderCancel).WithMessageTtl(TimeSpan.FromSeconds(3)));
            bus.Bind(exOrder, qOrder, "order");

            while (Console.ReadLine().ToLower() != "exit")
            {
                bus.Publish(exOrder, "order", false, user);
                Console.WriteLine($"{DateTimeOffset.Now.ToUnixTimeMilliseconds()} 发送成功");
            }
        }
        #endregion

        #region PubSub Size
        static void EasyNetQPubSubSize()
        {
            var user = new Message<UserInfo2>(new UserInfo2
            {
                Id = 13,
                Name = "Jack13",
                Gender = Gender.Male,
                IsDeleted = true
            });

            Console.Write("发送内容：");
            using var bus = RabbitHutch.CreateBus(_str).Advanced;

            var exSize = bus.ExchangeDeclare("jack.ex.size", ExchangeType.Fanout);
            var qSize = bus.QueueDeclare("jack.q.size", cfg => cfg.WithMaxLength(3).WithMaxPriority(2));
            bus.Bind(exSize, qSize, "size");

            while (Console.ReadLine().ToLower() != "exit")
            {
                bus.Publish(exSize, "size", false, user);
                Console.WriteLine($"{DateTimeOffset.Now.ToUnixTimeMilliseconds()} 发送成功");
            }
        }
        #endregion

        #endregion
    }
}
