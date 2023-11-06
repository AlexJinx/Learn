using MessagePack;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using MK.Common;

using SignalR_Service_Demo.Codes;
using SignalR_Service_Demo.Hubs;
using SignalR_Service_Demo.Models.SysModels;

using System.Security.Cryptography;

#region Services

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mkSite = builder.Configuration.GetSection("MKSite");
builder.Services.Configure<MKSiteModel>(mkSite);

#region SignalR

builder.Services.AddSignalR().AddMessagePackProtocol(options =>
{
    options.SerializerOptions = MessagePackSerializerOptions
                .Standard
                .WithResolver(MessagePack.Resolvers.StandardResolver.Instance)
                .WithSecurity(MessagePackSecurity.UntrustedData);
});

#endregion

#region Auth

builder.Services.AddAuthentication(opts =>
{
    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(optinos =>
{
    var rsa = RSA.Create();
    rsa.ImportPublicKey(RSAKeyType.Pkcs1, mkSite["JwtPub"]);
    optinos.RequireHttpsMetadata = false;

    optinos.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new RsaSecurityKey(rsa),

        ValidateIssuer = true,
        ValidIssuer = mkSite["JwtIss"],

        ValidateAudience = true,
        ValidAudience = mkSite["JwtAud"],

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };

    optinos.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            // 默认从URL中取Token,也可以配置从Header中取（好像不行...）
            //var accessToken = context.Request.Headers["Authorization"];
            Console.WriteLine(accessToken);
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/testHub")))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

#endregion

#if DEBUG
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1.0", new OpenApiInfo { Title = "测试 SignalR API", Version = "v1.0" });
    c.SchemaFilter<SwaggerIgnoreExFilter>();

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
       {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
       }
    });
});
#endif

#endregion

#region App
var app = builder.Build();

#if DEBUG
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.DocumentTitle = "测试 SignalR API v1.0";
    c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "测试 SignalR API v1.0");
});
#else
app.UseHttpsRedirection();
#endif



app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseEndpoints(endpoints => endpoints.MapHub<TestHub>("/testHub"));
app.MapGet("/", () => "WelCome");

await app.RunAsync();

#endregion


