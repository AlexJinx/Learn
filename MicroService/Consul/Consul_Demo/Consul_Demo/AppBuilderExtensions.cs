using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consul_Demo
{
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, ConsulServiceOptions consulServiceOptions)
        {
            var consulCli = new ConsulClient(c =>
            {
                // consul服务地址，默认安装consul后端口为8500
                c.Address = new Uri(consulServiceOptions.ConsulAddress);
            });

            var registration = new AgentServiceRegistration
            {
                ID = "ConsulDemo",
                Name = consulServiceOptions.ServiceName,    // 服务名
                Address = consulServiceOptions.ServiceIP,   // 服务绑定IP(也就是你这个项目运行的ip地址)
                Port = consulServiceOptions.ServicePort,    // 服务绑定端口(也就是你这个项目运行的端口)
                Check = new AgentServiceCheck
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                    Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔
                    HTTP = consulServiceOptions.ServiceHealthCheck,//健康检查地址
                    Timeout = TimeSpan.FromSeconds(5)
                }
            };

            consulCli.Agent.ServiceRegister(registration).Wait();
            return app;
        }
    }
}
