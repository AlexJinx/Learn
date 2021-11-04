using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Common
{
    public static class Helper
    {
        public static IConnectionFactory CreateConnectionFactory(string hostName, int port, string userName, string password)
        {
            return new ConnectionFactory
            {
                HostName = hostName,    //IP地址
                Port = port,            //端口号
                UserName = userName,    //用户账号
                Password = password     //用户密码
            };
        }
    }
}
