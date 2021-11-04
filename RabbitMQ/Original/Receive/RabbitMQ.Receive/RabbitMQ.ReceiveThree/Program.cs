using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMQ.ReceiveThree
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");
            IConnectionFactory connFactory = new ConnectionFactory//创建连接工厂对象
            {
                HostName = "119.29.9.146",//IP地址
                Port = 5672,//端口号
                UserName = "admin",//用户账号
                Password = "admin"//用户密码
            };
            using (IConnection conn = connFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    string queueName = String.Empty;
                    if (args.Length > 0)
                        queueName = args[0];
                    else
                        queueName = "QuDLX";
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
                        var cc = 1;
                        byte[] message = ea.Body.ToArray();//接收到的消息

                        string msg = Encoding.UTF8.GetString(message);



                        var deliveryTag = ea.DeliveryTag;

                        Console.WriteLine($"接收到信息为:{deliveryTag}--{msg}");

                        channel.BasicNack()

                        if (msg == "11")
                            channel.BasicReject(deliveryTag, true);

                    };
                    //消费者开启监听
                    channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
                    Console.ReadKey();
                }
            }
        }
    }
}
