using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace abbitMQ.DLX_Send
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "119.29.9.146";
            factory.Port = 5672;
            //factory.VirtualHost = "/";
            factory.UserName = "admin";
            factory.Password = "admin";

            var exchangeA = "changeA";
            var routeA = "routeA";
            var queueA = "queueA";

            var exchangeD = "changeD";
            var routeD = "routeD";
            var queueD = "queueD";

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchangeD, type: "fanout", durable: true, autoDelete: false);
                    channel.QueueDeclare(queueD, durable: true, exclusive: false, autoDelete: false);
                    channel.QueueBind(queueD, exchangeD, routeD);

                    channel.ExchangeDeclare(exchangeA, type: "fanout", durable: true, autoDelete: false);
                    channel.QueueDeclare(queueA, durable: true, exclusive: false, autoDelete: false, arguments: new Dictionary<string, object> {
                                         { "x-dead-letter-exchange",exchangeD}, //设置当前队列的DLX
                                         { "x-dead-letter-routing-key",routeD}, //设置DLX的路由key，DLX会根据该值去找到死信消息存放的队列
                                         { "x-message-ttl",10000} //设置消息的存活时间，即过期时间
                                         });
                    channel.QueueBind(queueA, exchangeA, routeA);


                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    //发布消息
                    channel.BasicPublish(exchange: exchangeA,
                                         routingKey: routeA,
                                         basicProperties: properties,
                                         body: Encoding.UTF8.GetBytes("message"));
                }
            }
        }
    }
}
