// See https://aka.ms/new-console-template for more information
using MK.Common;

using Redis_Delay_Queue;

Console.WriteLine("Hello, World!");
Test.Start();

MKRedis.SetConn("101.34.61.66:6379,name=wine_api_dev");
await MKRedis.AddSortedSetAsync("1", 123213213, DateTimeOffset.Now.AddMinutes(1).ToUnixTimeMilliseconds());

Console.ReadKey();