using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBJ.MicroService.APIGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddJsonFile("Ocelot.json", false, true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                    var port = builder["ServicePort"];
                    webBuilder.UseUrls($"http://*:{port}").UseStartup<Startup>();
                });
    }
}