using EasyNetQ;

namespace TestMQ.Entity
{
    public enum Gender
    {
        Unknown = 0,
        Male = 1,
        Female = 2
    }

    [Queue("jack.q2", ExchangeName = "jack.ex2")]
    public class UserInfo
    {
        public ulong Id { get; set; }

        public string Name { get; set; }

        public Gender Gender { get; set; }

        public bool IsDeleted { get; set; }
    }

    public class UserInfo2
    {
        public ulong Id { get; set; }

        public string Name { get; set; }

        public Gender Gender { get; set; }

        public bool IsDeleted { get; set; }
    }
}
