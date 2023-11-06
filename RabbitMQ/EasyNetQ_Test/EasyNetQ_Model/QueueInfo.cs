namespace EasyNetQ_Model
{
    public class QueueInfo
    {
        public string? DelayExChangeName { get; set; }

        public string? ExChangeName { get; set; }

        public string? ExChangeType { get; set; }

        public string? ExChangeRouteKey { get; set; }

        public string? QueueName { get; set; }

        public int Expiry { get; set; }

        public IEnumerable<string>? Msg { get; set; }

        public string? MsgRouteKey { get; set; }

        public ConsoleColor ConsoleColor { get; set; }
    }
}
