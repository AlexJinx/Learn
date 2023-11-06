using MK.Common;

using Newtonsoft.Json;

using StackExchange.Redis;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redis_Delay_Queue;
public class Test
{
    //创建一个redis连接对象
    private static readonly ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("101.34.61.66:6379,name=wine_api_dev");
    //获取一个redis数据库对象
    private static readonly IDatabase db = redis.GetDatabase();
    //定义一个延时队列的键名
    private static readonly string queueKey = "delay_queue";
    //定义一个分布式锁的键名
    private static readonly string lockKey = "delay_queue_lock";
    //定义一个分布式锁的过期时间（毫秒）
    private static readonly int lockTimeout = 1000;

    public static void Start()
    {
        //模拟生产者，向延时队列中添加10个任务，每个任务的延时时间为1到10秒
        for (int i = 1; i <= 10; i++)
        {
            //创建一个任务对象，包含任务ID和任务内容
            var task = new TaskItem()
            {
                Id = Guid.NewGuid().ToString(),
                Content = $"Task {i}"
            };
            //将任务对象序列化为JSON字符串
            var value = task.ToJson();
            //计算任务的执行时间（当前时间加上延时时间）
            var score = DateTime.Now.AddSeconds(i).Ticks;
            //将任务添加到延时队列中，以执行时间作为分数，以任务内容作为值
            db.SortedSetAdd(queueKey, value, score);
            Console.WriteLine($"生产者：添加了任务 {task.Content}，执行时间为 {new DateTime(score):HH:mm:ss}");
        }

        Task.Delay(10000).Wait();

        //模拟消费者，从延时队列中取出到期的任务，并执行
        //while (true)
        //{
        //    //获取当前时间的时间戳
        //    var now = DateTime.Now.Ticks;
        //    //从延时队列中查询分数小于等于当前时间的所有任务（即到期的任务）
        //    var tasks = db.SortedSetRangeByScore(queueKey, 0, now);
        //    if (tasks.Length == 0)
        //    {
        //        //如果没有到期的任务，等待一秒后继续查询
        //        Console.WriteLine("消费者：没有到期的任务，等待一秒...");
        //        Task.Delay(1000).Wait();
        //        continue;
        //    }
        //    foreach (var task in tasks)
        //    {
        //        //尝试获取分布式锁，避免多个消费者同时处理同一个任务
        //        var lockValue = Guid.NewGuid().ToString();
        //        if (db.LockTake(lockKey, lockValue, TimeSpan.FromMilliseconds(lockTimeout)))
        //        {
        //            try
        //            {
        //                //如果获取到锁，再次确认任务是否在队列中（避免已被其他消费者处理）
        //                if (db.SortedSetRank(queueKey, task).HasValue)
        //                {
        //                    //如果任务仍在队列中，将其移除
        //                    db.SortedSetRemove(queueKey, task);
        //                    //将任务反序列化为对象
        //                    var taskItem = JsonConvert.DeserializeObject<TaskItem>(task);
        //                    //执行任务逻辑（这里只是打印一句话）
        //                    Console.WriteLine($"消费者：执行了任务 {taskItem.Content}，当前时间为 {DateTime.Now:HH:mm:ss}");
        //                }
        //            }
        //            finally
        //            {
        //                //释放分布式锁
        //                db.LockRelease(lockKey, lockValue);
        //            }
        //        }
        //    }
        //}
    }
}

public class TaskItem
{
    public string Id { get; set; }
    public string Content { get; set; }
}