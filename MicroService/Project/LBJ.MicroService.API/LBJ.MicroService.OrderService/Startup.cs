using ConsulBuilder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBJ.MicroService.OrderService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            #region ����Consul
            var ip = Configuration["ip"];
            var port = Configuration["port"];

            //��ȡ����Consul�����ýڵ�
            var consulSection = Configuration.GetSection("Consul");
            var consulOption = new ConsulServiceOptions
            {
                ServiceName = consulSection["ServiceName"],
                ServiceIP = consulSection["ServiceIP"],
                ServicePort = Convert.ToInt32(consulSection["ServicePort"]),
                ServiceHealthCheck = consulSection["ServiceHealthCheck"],
                ConsulAddress = consulSection["ConsulAddress"]
            };

            //�����в����������
            if (!string.IsNullOrWhiteSpace(ip) && !string.IsNullOrWhiteSpace(port))
            {
                consulOption.ServiceHealthCheck = $"http://{ip}:{port}/api/HealthCheck";
                consulOption.ServiceIP = ip;
                consulOption.ServicePort = Convert.ToInt32(port);
            }

            //ע��consul����
            app.AddConsul(consulOption);
            #endregion

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
