using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMQ.ReceiveTwo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Consumer Start");
            IConnectionFactory connFactory = new ConnectionFactory//创建连接工厂对象
            {
                HostName = "101.34.61.66",//IP地址
                Port = 5672,//端口号
                UserName = "admin",//用户账号
                Password = "admin"//用户密码
            };
            using (IConnection conn = connFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    string queueName = "QuDLX";
                    //声明一个队列
                    channel.QueueDeclare(
                      queue: queueName,//消息队列名称
                      durable: true,//是否缓存
                      exclusive: false,
                      autoDelete: false,
                      arguments: null
                       );
                    //创建消费者对象
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        byte[] message = ea.Body.ToArray();//接收到的消息
                        Console.WriteLine("接收到信息为:" + Encoding.UTF8.GetString(message));
                    };
                    //消费者开启监听
                    channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
                    Console.ReadKey();
                }
            }
        }

        //static void Main(string[] args)
        //{
        //    Console.WriteLine("Start");
        //    IConnectionFactory conFactory = new ConnectionFactory
        //    {
        //        HostName = "101.34.61.66",//IP地址
        //        Port = 5672,//端口号
        //        UserName = "admin",//用户账号
        //        Password = "admin"//用户密码
        //    };
        //    using (IConnection conn = conFactory.CreateConnection())
        //    {
        //        using (IModel channel = conn.CreateModel())
        //        {
        //            string queueName = string.Empty;
        //            queueName = "QuDLX";
        //            //声明一个队列
        //            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        //            //告诉Rabbit每次只能向消费者发送一条信息,再消费者未确认之前,不再向他发送信息
        //            channel.BasicQos(0, 1, false);

        //            //创建消费者对象
        //            var consumer = new EventingBasicConsumer(channel);
        //            consumer.Received += (model, ea) =>
        //            {
        //                byte[] message = ea.Body.ToArray();//接收到的消息
        //                //string msgStr = Encoding.UTF8.GetString(message);
        //                Console.WriteLine("接收到信息为:" + Encoding.UTF8.GetString(message));

        //                // 手动确认消息【参数说明：参数一：该消息的index；参数二：是否批量应答，true批量确认小于当前id的消息】

        //                //if (msgStr == "1")
        //                //{
        //                //    channel.BasicReject(ea.DeliveryTag, false);
        //                //}
        //                //else
        //                //{
        //                //    channel.BasicAck(ea.DeliveryTag, true);
        //                //}
        //            };
        //            // 消费者开启监听
        //            // 将autoAck设置false 关闭自动确认
        //            channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
        //            Console.ReadKey();
        //        }
        //    }
        //}
    }
}
