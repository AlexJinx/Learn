using MK.Common;
using StackExchange.Redis;
using System;

namespace TestRedis.PubSub
{
    class Program
    {
        static void Main(string[] args)
        {
            MKRedis.SetConn("192.168.16.192:5014,name=expire_test");

            MKRedis.GetDb(3).StringSet("order:1", "1", TimeSpan.FromSeconds(2));
            MKRedis.GetDb(3).StringSet("order:2", "2", TimeSpan.FromSeconds(4));
            MKRedis.GetDb(3).StringSet("order:3", "3", TimeSpan.FromSeconds(6));
            MKRedis.GetDb(3).StringSet("order:4", "4", TimeSpan.FromSeconds(8));
            MKRedis.GetDb(3).StringSet("order:5", "5", TimeSpan.FromSeconds(10));
            MKRedis.GetDb(3).StringSet("order:6", "6", TimeSpan.FromSeconds(12));
            MKRedis.GetDb(3).StringSet("order:7", "7", TimeSpan.FromSeconds(14));
            MKRedis.GetDb(3).StringSet("order:8", "8", TimeSpan.FromSeconds(14));
            MKRedis.GetDb(3).StringSet("order:9", "9", TimeSpan.FromSeconds(12));

            var sub = MKRedis.Instance.GetSubscriber();
            sub.Subscribe("__keyevent@3__:expired", subHandle);

            Console.ReadLine();
        }

        static void subHandle(RedisChannel chn, RedisValue val)
        {
            Console.WriteLine($"{DateTimeOffset.Now.ToUnixTimeMilliseconds()} 频道：{chn} 收到消息：{val}");
        }
    }
}
