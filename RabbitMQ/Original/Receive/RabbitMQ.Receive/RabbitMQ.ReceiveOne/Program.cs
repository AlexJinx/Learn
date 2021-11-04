using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Common;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace RabbitMQ.ReceiveOne
{
    class Program
    {
        private const string QUEUE_NAME = "QuDLX";
        private const string IP_ADDRESS = "119.29.9.146";
        private const int PORT = 5672;//RabbitMQ 服务端默认端口5672；
        private const string USER_NAME = "ammin";
        private const string PASSWORD = "ammin";

        static void Main(string[] args)
        {

            Consumer();

            Console.ReadKey();

            #region code

            //Console.WriteLine("Start");
            //string queueName = "queue1";

            //var conFactory = Helper.CreateConnectionFactory("119.29.9.146", 5672, "guest", "guest");

            //var conn = conFactory.CreateConnection();
            //var channel = conn.CreateModel();

            //// 声明队列【参数说明：参数一：队列名称，参数二：是否持久化；参数三：是否独占模式；参数四：消费者断开连接时是否删除队列；参数五：消息其他参数】
            //channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            //channel.BasicQos(0, 1, false);

            ////创建消费者对象
            //var consumer = new EventingBasicConsumer(channel);
            //consumer.Received += (model, ea) =>
            //{
            //    byte[] message = ea.Body.ToArray();//接收到的消息
            //    Console.WriteLine("接收到信息为:" + Encoding.UTF8.GetString(message));

            //    channel.BasicAck(ea.DeliveryTag, true);
            //};

            ////消费者开启监听
            //channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            //Console.ReadKey();

            //channel.Close();
            //conn.Close();

            #endregion


            #region code
            //using (IConnection conn = conFactory.CreateConnection())
            //{
            //    using (IModel channel = conn.CreateModel())
            //    {
            //        string queueName = string.Empty;
            //        if (args.Length > 0)
            //            queueName = args[0];
            //        else
            //            queueName = "queue1";
            //        // 声明一个队列
            //        // 声明队列【参数说明：参数一：队列名称，参数二：是否持久化；参数三：是否独占模式；参数四：消费者断开连接时是否删除队列；参数五：消息其他参数】
            //        channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            //        //创建消费者对象
            //        var consumer = new EventingBasicConsumer(channel);
            //        consumer.Received += (model, ea) =>
            //        {
            //            byte[] message = ea.Body.ToArray();//接收到的消息
            //            Console.WriteLine("接收到信息为:" + Encoding.UTF8.GetString(message));
            //        };
            //        //消费者开启监听
            //        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            //        Console.ReadKey();
            //    }
            //}
            #endregion
        }

        public static void Consumer()
        {
            IConnection con = null;
            IModel channel = null;

            try
            {
                //01.创建factory
                ConnectionFactory factory = new ConnectionFactory
                {
                    HostName = "119.29.9.146",//IP地址
                    Port = 5672,//端口号
                    UserName = "admin",//用户账号
                    Password = "admin"//用户密码
                };

                //02.创建连接
                con = factory.CreateConnection();

                //03.创建channel
                channel = con.CreateModel();

                //创建一个持久的、非排他的、非自动删除的队列
                channel.QueueDeclare(QUEUE_NAME, true, false, false, null);

                //队列最大接收未被ack的消息的个数
                channel.BasicQos(0, 1, false);

                //04.创建消费者-监听方式
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    Run(body.ToArray());
                    channel.BasicAck(ea.DeliveryTag, false);
                };
                channel.BasicConsume(QUEUE_NAME, false, consumer);

            }
            catch (IOException ioE)
            {
                throw;
            }
            catch (SocketException socketEx)//RabbitMQ 用TCP协议，这里除了socket异常
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                //05.关闭资源
                if (channel != null)
                    channel.Close();
                if (con != null)
                    con.Close();
            }
        }

        private static void Run(byte[] body)
        {
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Received {0}", message);
        }

    }
}
