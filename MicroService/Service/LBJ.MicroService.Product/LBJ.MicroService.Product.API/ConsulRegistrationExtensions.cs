using Consul;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBJ.MicroService.Product.API
{
    public static class ConsulRegistrationExtensions
    {
        public static IApplicationBuilder AddConsul(this IApplicationBuilder app, ConsulServiceOptions options)
        {
            var consulClient = new ConsulClient(cli =>
            {
                // consul服务地址，默认安装consul后端口为8500
                cli.Address = new Uri(options.ConsulAddress);
            });

            var regist = new AgentServiceRegistration()
            {
                ID = "Product",
                Name = options.ServiceName,     // 服务名
                Address = options.ServiceIP,    // 服务绑定IP(也就是你这个项目运行的ip地址)
                Port = options.ServicePort,     // 服务绑定端口(也就是你这个项目运行的端口)
                Check = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),   //服务启动多久后注册
                    Interval = TimeSpan.FromSeconds(10),    //健康检查时间间隔
                    HTTP = options.ServiceHealthCheck,      //健康检查地址
                    Timeout = TimeSpan.FromSeconds(5)
                }
            };

            // 服务注册
            consulClient.Agent.ServiceRegister(regist).Wait();
            return app;
        }
    }
}
