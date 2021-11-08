using EasyNetQ;
using EasyNetQ.Topology;

using MK.Common;

using System;

namespace TestMQ.Size.Send
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
            Console.WriteLine("生产者");

            EasyNetQSizeLimit();
        }

        #region EasyNetQSizeLimit
        static void EasyNetQSizeLimit()
        {
            Console.WriteLine("等待发送内容");

            using var bus = RabbitHutch.CreateBus(_str).Advanced;

            // 失败队列，进入定长队列失败，通知消费者，消费完后自动删除队列
            var exSizeFailure = bus.ExchangeDeclare("jack.ex.size.failure", ExchangeType.Direct);
            var qSizeFailure = bus.QueueDeclare("jack.q.size.failure", cfg => cfg.AsAutoDelete(true));
            bus.Bind(exSizeFailure, qSizeFailure, "sizeLimit");

            // 定长队列，数据往此队列发（没有溢出的，表示进入队列了）
            var exSizeLimit = bus.ExchangeDeclare("jack.ex.size.limit", ExchangeType.Direct);
            var qSizeLimit = bus.QueueDeclare("jack.q.size.limit", cfg => cfg.WithDeadLetterExchange(exSizeFailure)
                                                                             .WithMaxLength(3)
                                                                             .WithArgument("x-overflow", "reject-publish-dlx"));
            bus.Bind(exSizeLimit, qSizeLimit, "sizeLimit");

            string msg;

            while ((msg = Console.ReadLine().ToLower()) != "exit")
            {
                var user = new UserInfo
                {
                    Id = MKHelper.GetUIntID(),
                    Name = msg,
                    At = DateTimeOffset.Now.ToUnixTimeMilliseconds()
                };

                bus.Publish(exSizeLimit, "sizeLimit", false, new Message<string>(user.ToJson()));
                Console.WriteLine($"{DateTimeOffset.Now.ToUnixTimeMilliseconds()} 发送成功：{user.ToJson()}");
            }
        }
        #endregion
    }
}
