using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Send
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Product Start");

            //创建连接工厂对象
            //IConnectionFactory conFactory = new ConnectionFactory
            //{
            //    HostName = "101.34.61.66",//IP地址
            //    Port = 5672,//端口号
            //    UserName = "admin",//用户账号
            //    Password = "admin"//用户密码
            //};

            IConnectionFactory conFactory = new ConnectionFactory
            {
                HostName = "192.168.16.192",//IP地址
                Port = 13817,//端口号
                UserName = "dev",//用户账号
                Password = "123456"//用户密码
            };


            //创建连接对象
            using (IConnection con = conFactory.CreateConnection())
            {
                //创建连接会话对象
                using (IModel channel = con.CreateModel())
                {
                    //声明死信交换机
                    channel.ExchangeDeclare("ExDLX", "direct", true, false);
                    channel.QueueDeclare("QuDLX", true, false, false);
                    channel.QueueBind("QuDLX", "ExDLX", "RouteB");

                    Dictionary<string, object> para = new Dictionary<string, object>();
                    para.Add("x-message-ttl", 6000);//TTL
                    para.Add("x-dead-letter-exchange", "ExDLX");//DLX
                    para.Add("x-dead-letter-routing-key", "RouteB");//routingKey

                    //声明交换机
                    channel.ExchangeDeclare("ExNo", "direct", true, false);
                    // 声明队列【参数说明：参数一：队列名称，参数二：是否持久化；参数三：是否独占模式；参数四：消费者断开连接时是否删除队列；参数五：消息其他参数】
                    channel.QueueDeclare("QuNo", true, false, false, para);
                    channel.QueueBind("QuNo", "ExNo", "RouteA", para);

                    var prop = channel.CreateBasicProperties();
                    prop.Persistent = true;

                    while (true)
                    {
                        var msg = Console.ReadLine();
                        byte[] body = Encoding.UTF8.GetBytes(msg);

                        channel.BasicPublish(exchange: "ExNo", routingKey: "RouteA", basicProperties: prop, body: body);
                    }
                }
            }




            ////创建连接对象
            //using (IConnection con = conFactory.CreateConnection())
            //{
            //    //创建连接会话对象
            //    using (IModel channel = con.CreateModel())
            //    {
            //        //string queueName = string.Empty;
            //        //if (para.Length > 0)
            //        //    queueName = para[0];
            //        //else
            //        //    queueName = "queue1";

            //        //DLX
            //        channel.ExchangeDeclare("exchange_dlx", "direct", true, false, null);
            //        //普通交换器
            //        channel.ExchangeDeclare("exchange_normal", "fanout", true, false, null);


            //        //参数设置
            //        Dictionary<string, object> para = new Dictionary<string, object>();
            //        para.Add("x-message-ttl", 10000);//TTL
            //        para.Add("x-dead-letter-exchange", "exchange_dlx");//DLX
            //        para.Add("x-dead-letter-routing-key", "routingkey");//routingKey


            //        //普通队列绑定
            //        channel.QueueDeclare("queue_normal", true, false, false, para);
            //        channel.QueueBind("queue_normal", "exchange_normal", null);


            //        //死信队列绑定
            //        channel.QueueDeclare("queue_dlx", true, false, false, para);
            //        channel.QueueBind("queue_dlx", "exchange_dlx", "routingkey", null);


            //        string message = "Hello Word!";
            //        var body = Encoding.UTF8.GetBytes(message);
            //        var properties = channel.CreateBasicProperties();
            //        properties.Persistent = true;

            //        channel.BasicPublish("exchange_normal", "", properties, body);

            //        // 声明一个队列
            //        // 声明队列【参数说明：参数一：队列名称，参数二：是否持久化；参数三：是否独占模式；参数四：消费者断开连接时是否删除队列；参数五：消息其他参数】
            //        //channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            //        //while (true)
            //        //{
            //        //    Console.WriteLine("消息内容:");
            //        //    string message = Console.ReadLine();
            //        //    //消息内容
            //        //    byte[] body = Encoding.UTF8.GetBytes(message);
            //        //    // 发送消息
            //        //    // 推送内容【参数说明：参数一：交换机名称；参数二：队列名称，参数三：消息的其他属性-路由的headers信息；参数四：消息主体】
            //        //    channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
            //        //    Console.WriteLine("成功发送消息:" + message);
            //        //}
            //    }
            //}
        }
    }
}
