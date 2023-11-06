using EasyNetQ.Topology;

using EasyNetQ_Client;

using EasyNetQ_Model;

Console.WriteLine("Start Enqueue");

await EnqueueTopci();
await Enqueue();

async Task Enqueue()
{
    List<QueueInfo> infos = new();
    infos.Add(new QueueInfo
    {
        DelayExChangeName = "jing.exA.delay",
        ExChangeName = "jing.exA.normal",
        ExChangeType = ExchangeType.Direct,
        QueueName = "jing.qA.normal",
        ExChangeRouteKey = "routeA",
        MsgRouteKey = "routeA",
        Expiry = 1000
    });
    infos.Add(new QueueInfo
    {
        DelayExChangeName = "jing.exA.delay",
        ExChangeName = "jing.exB.normal",
        ExChangeType = ExchangeType.Direct,
        QueueName = "jing.qB.normal",
        ExChangeRouteKey = "routeB",
        MsgRouteKey = "routeB",
        Expiry = 1000
    });
    infos.Add(new QueueInfo
    {
        DelayExChangeName = "jing.exA.delay",
        ExChangeName = "jing.exC.normal",
        ExChangeType = ExchangeType.Direct,
        QueueName = "jing.qC.normal",
        ExChangeRouteKey = "routeC",
        MsgRouteKey = "routeC",
        Expiry = 1000
    });

    foreach (var item in infos)
    {
        item.Msg = await Task.Run(() =>
        {
            List<string> msg = new();
            for (int i = 0; i < 1000; i++)
            {
                msg.Add($"{item.QueueName}-{i}");
            };
            return msg;
        });
    }

    Parallel.ForEach(infos, item => { MQClient.Enqueue(item); });
}

async Task EnqueueTopci()
{
    List<QueueInfo> infos = new();
    infos.Add(new QueueInfo
    {
        DelayExChangeName = "jing.exAA.delay",
        ExChangeName = "jing.exTAA.normal",
        ExChangeType = ExchangeType.Topic,
        QueueName = "jing.qA.normal",
        ExChangeRouteKey = "*.order.*",
        MsgRouteKey = "123213.order.vxvvxc",
        Expiry = 1000
    });
    infos.Add(new QueueInfo
    {
        DelayExChangeName = "jing.exA.delay",
        ExChangeName = "jing.exTB.normal",
        ExChangeType = ExchangeType.Topic,
        QueueName = "jing.qB.normal",
        ExChangeRouteKey = "*.ticket.*",
        MsgRouteKey = "hscc.ticket.route",
        Expiry = 1000
    });
    infos.Add(new QueueInfo
    {
        DelayExChangeName = "jing.exA.delay",
        ExChangeName = "jing.exTC.normal",
        ExChangeType = ExchangeType.Topic,
        QueueName = "jing.qC.normal",
        ExChangeRouteKey = "hscc.#",
        MsgRouteKey = "hscc.other.route",
        Expiry = 1000
    });

    foreach (var item in infos)
    {
        item.Msg = await Task.Run(() =>
        {
            List<string> msg = new();
            for (int i = 0; i < 50; i++)
            {
                msg.Add($"{item.QueueName}-{i}");
            };
            return msg;
        });
    }

    Parallel.ForEach(infos, item => { MQClient.Enqueue(item); });
}