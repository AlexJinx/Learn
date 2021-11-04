using Consul;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsulBuilder
{
    public static class ConsulRegistrationExtensions
    {
        public static IApplicationBuilder AddConsul(this IApplicationBuilder app, ConsulServiceOptions consulServiceOptions)
        {
            var consulClient = new ConsulClient(x =>
            {
                // consul服务地址，默认安装consul后端口为8500
                x.Address = new Uri(consulServiceOptions.ConsulAddress);
            });

            var registration = new AgentServiceRegistration()
            {
                ID = Guid.NewGuid().ToString(),
                Name = consulServiceOptions.ServiceName,// 服务名
                Address = consulServiceOptions.ServiceIP, // 服务绑定IP(也就是你这个项目运行的ip地址)
                Port = consulServiceOptions.ServicePort, // 服务绑定端口(也就是你这个项目运行的端口)
                Check = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                    Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔
                    HTTP = consulServiceOptions.ServiceHealthCheck,//健康检查地址
                    Timeout = TimeSpan.FromSeconds(5)
                }
            };

            // 服务注册
            consulClient.Agent.ServiceRegister(registration).Wait();
            return app;
        }
    }
}
