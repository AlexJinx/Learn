using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TaskAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            Console.WriteLine(1.ToString() + "    线程ID    " + Thread.CurrentThread.ManagedThreadId);

            await ExecLongTimeOptionAsync();
            //ExecLongTimeOptionAsyncTwo();

            Console.WriteLine(2.ToString() + "    线程ID    " + Thread.CurrentThread.ManagedThreadId);

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = 33,
                Summary = "asf"
            }).ToArray();
        }

        [HttpPost]
        public async Task ExecLongTimeOptionAsync()
        {
            Console.WriteLine(3.ToString() + "    线程ID    " + Thread.CurrentThread.ManagedThreadId);
            await Task.Run(() =>
            {
                Console.WriteLine(4.ToString() + "    线程ID    " + Thread.CurrentThread.ManagedThreadId);
                using (var file = new FileStream(@"D:\\TestOne.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    for (int i = 0; i < 30001; i++)
                    {
                        string row = i.ToString() + "/n";
                        file.Write(System.Text.Encoding.UTF8.GetBytes(row));
                    }
                }
                Console.WriteLine(5.ToString() + "    线程ID    " + Thread.CurrentThread.ManagedThreadId);
            });

            await Task.Run(() =>
             {
                 Console.WriteLine(6.ToString() + "    线程ID    " + Thread.CurrentThread.ManagedThreadId);
                 using (var file = new FileStream(@"D:\\TestTwo.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                 {
                     for (int i = 0; i < 30001; i++)
                     {
                         string row = i.ToString() + "/n";
                         file.Write(System.Text.Encoding.UTF8.GetBytes(row));
                     }
                 }
                 Console.WriteLine(7.ToString() + "    线程ID    " + Thread.CurrentThread.ManagedThreadId);
             });
            Console.WriteLine(8.ToString() + "    线程ID    " + Thread.CurrentThread.ManagedThreadId);
        }

        [HttpPost("two")]
        public void ExecLongTimeOptionAsyncTwo()
        {
            Console.WriteLine(3.ToString() + "    线程ID    " + Thread.CurrentThread.ManagedThreadId);
            Task.Run(() =>
            {
                Console.WriteLine(4.ToString() + "    线程ID    " + Thread.CurrentThread.ManagedThreadId);
                using (var file = new FileStream(@"D:\\TestOne.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    for (int i = 0; i < 30001; i++)
                    {
                        string row = i.ToString() + "/n";
                        file.Write(System.Text.Encoding.UTF8.GetBytes(row));
                    }
                }
                Console.WriteLine(5.ToString() + "    线程ID    " + Thread.CurrentThread.ManagedThreadId);
            });

            Task.Run(() =>
            {
                Console.WriteLine(6.ToString() + "    线程ID    " + Thread.CurrentThread.ManagedThreadId);
                using (var file = new FileStream(@"D:\\TestTwo.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    for (int i = 0; i < 30001; i++)
                    {
                        string row = i.ToString() + "/n";
                        file.Write(System.Text.Encoding.UTF8.GetBytes(row));
                    }
                }
                Console.WriteLine(7.ToString() + "    线程ID    " + Thread.CurrentThread.ManagedThreadId);
            });
            Console.WriteLine(8.ToString() + "    线程ID    " + Thread.CurrentThread.ManagedThreadId);
        }
    }
}
