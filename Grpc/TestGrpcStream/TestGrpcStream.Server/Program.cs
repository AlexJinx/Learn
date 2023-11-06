using TestGrpcStream.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<MKStreamService>();
app.MapGet("/", () => "GRPC Stream");

Console.WriteLine("GRPC Stream ·þÎñ¶Ë");

app.Run();
