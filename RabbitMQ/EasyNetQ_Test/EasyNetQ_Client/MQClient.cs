using EasyNetQ;
using EasyNetQ.Topology;

using EasyNetQ_Model;

namespace EasyNetQ_Client
{
    public class MQClient
    {
        private const string _str = "host=81.70.118.191:5672;virtualhost=/vjing;username=jing;password=123456";


        #region Enqueue

        public static void Enqueue()
        {
            using var bus = RabbitHutch.CreateBus(_str);

            var exDelay = bus.Advanced.ExchangeDeclare("jing.exA.delay", cfg => cfg.AsDelayedExchange(ExchangeType.Direct));

            var exA = bus.Advanced.ExchangeDeclare("jing.exA.normal", ExchangeType.Direct);
            var exB = bus.Advanced.ExchangeDeclare("jing.exB.normal", ExchangeType.Direct);
            var exC = bus.Advanced.ExchangeDeclare("jing.exC.normal", ExchangeType.Direct);

            var qA = bus.Advanced.QueueDeclare("jing.qA.normal");
            var qB = bus.Advanced.QueueDeclare("jing.qB.normal");
            var qC = bus.Advanced.QueueDeclare("jing.qC.normal");

            bus.Advanced.Bind(exDelay, exA, "routeA");
            bus.Advanced.Bind(exDelay, exB, "routeB");
            bus.Advanced.Bind(exDelay, exC, "routeC");

            bus.Advanced.Bind(exA, qA, "routeA");
            bus.Advanced.Bind(exB, qB, "routeB");
            bus.Advanced.Bind(exC, qC, "routeC");

            for (int i = 0; i < 10000; i++)
            {
                var msgPropA = new MessageProperties();
                msgPropA.Headers.Add("x-delay", 5000);
                bus.Advanced.Publish(exDelay, "routeA", false, new Message<string>($"exA-qA-{i}", msgPropA));

                var msgPropB = new MessageProperties();
                msgPropB.Headers.Add("x-delay", 10000);
                bus.Advanced.Publish(exDelay, "routeB", false, new Message<string>($"exA-qA-{i}", msgPropB));

                var msgPropC = new MessageProperties();
                msgPropC.Headers.Add("x-delay", 15000);
                bus.Advanced.Publish(exDelay, "routeC", false, new Message<string>($"exA-qA-{i}", msgPropC));
            }
        }

        public static void Enqueue(QueueInfo info)
        {

            Console.WriteLine(info.ExChangeType);

            using var bus = RabbitHutch.CreateBus(_str);

            var exDelay = bus.Advanced.ExchangeDeclare(info.DelayExChangeName, cfg => cfg.AsDelayedExchange(info.ExChangeType));
            var exNormal = bus.Advanced.ExchangeDeclare(info.ExChangeName, info.ExChangeType);
            bus.Advanced.Bind(exDelay, exNormal, info.ExChangeRouteKey);

            var queueNormal = bus.Advanced.QueueDeclare(info.QueueName);
            bus.Advanced.Bind(exNormal, queueNormal, info.ExChangeRouteKey);

            var msgProp = new MessageProperties();
            msgProp.Headers.Add("x-delay", info.Expiry);
            foreach (var item in info.Msg!)
            {
                bus.Advanced.Publish(exDelay, info.MsgRouteKey, false, new Message<string>(item, msgProp));
            }
        }

        #endregion


        #region Dequeue

        public static void Dequeue(QueueInfo info)
        {
            using var bus = RabbitHutch.CreateBus(_str);

            var qA = bus.Advanced.QueueDeclare(info.QueueName);
            bus.Advanced.Consume<string>(qA, (msg, msgInfo) =>
            {
                Console.BackgroundColor = info.ConsoleColor;
                Console.WriteLine(msg.Body);
                Console.ResetColor();
            });

            Console.ReadKey();
        }

        public static void Dequeue()
        {
            using var bus = RabbitHutch.CreateBus(_str);

            var qA = bus.Advanced.QueueDeclare("jing.qA.normal");
            var qB = bus.Advanced.QueueDeclare("jing.qB.normal");
            var qC = bus.Advanced.QueueDeclare("jing.qC.normal");

            bus.Advanced.Consume<string>(qA, (msg, info) =>
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(msg.Body);
                Console.ResetColor();
            });
            bus.Advanced.Consume<string>(qB, (msg, info) =>
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.WriteLine(msg.Body);
                Console.ResetColor();
            });
            bus.Advanced.Consume<string>(qC, (msg, info) =>
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.WriteLine(msg.Body);
                Console.ResetColor();
            });

            Console.ReadKey();
        }

        #endregion

    }
}
