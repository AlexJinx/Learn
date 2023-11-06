using Hangfire;
using Hangfire.Redis;

using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddHangfire(config => config.UseRedisStorage("81.70.118.191:6379,name=cjly_wq1937_api_dev", new RedisStorageOptions
{
    Db = 1,
    Prefix = "test:task:"
}));

builder.Services.AddHangfireServer(opts =>
{
    opts.ServerName = "aliyun-cdb1";
    opts.Queues = new[] { "wechat", "default" };
    //opts.WorkerCount = Environment.ProcessorCount * 5;
    //opts.SchedulePollingInterval = TimeSpan.FromMinutes(1);
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
