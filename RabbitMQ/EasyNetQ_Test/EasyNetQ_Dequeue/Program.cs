using EasyNetQ_Client;

using EasyNetQ_Model;

Dequeue();
//DequeueTopic();

void Dequeue()
{
    List<QueueInfo> infos = new();
    infos.Add(new QueueInfo { QueueName = "jing.qA.normal", ConsoleColor = ConsoleColor.Magenta });
    infos.Add(new QueueInfo { QueueName = "jing.qB.normal", ConsoleColor = ConsoleColor.Yellow });
    infos.Add(new QueueInfo { QueueName = "jing.qC.normal", ConsoleColor = ConsoleColor.Cyan });

    Parallel.ForEach(infos, item => { MQClient.Dequeue(item); });
}

void DequeueTopic()
{
    List<QueueInfo> infos = new();
    infos.Add(new QueueInfo { QueueName = "jing.qA.normal", ConsoleColor = ConsoleColor.Magenta });
    infos.Add(new QueueInfo { QueueName = "jing.qB.normal", ConsoleColor = ConsoleColor.Yellow });
    infos.Add(new QueueInfo { QueueName = "jing.qC.normal", ConsoleColor = ConsoleColor.Cyan });

    Parallel.ForEach(infos, item => { MQClient.Dequeue(item); });
}