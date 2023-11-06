using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.WebEncoders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using Morin.API.Models;

using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

var sectionSite = builder.Configuration.GetSection("mrCfg");

builder.WebHost.ConfigureKestrel(option =>
{
    option.Listen(IPAddress.Any, int.Parse(sectionSite["Port"]));
});

// Add services to the container.

// 直接使用 HtmlEncoder 会出现 中文 和 全角符号 被 HTML 转义的问题，若不需要转义，使用以下方法：
builder.Services.Configure<WebEncoderOptions>(opts =>
{
    opts.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.Configure<MorinConfigModel>(sectionSite);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, option =>
{
    option.RequireHttpsMetadata = true;
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(sectionSite["JwtSecret"])),

        ValidateIssuer = true,
        ValidIssuer = sectionSite["JwtIss"],

        ValidateAudience = true,
        ValidAudience = sectionSite["JwtAud"],

        ValidateLifetime = true,

        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("MrCors", policy =>
    {
        policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
    });
});



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1.0", new OpenApiInfo { Title = "Morin API", Version = "v1.0" });
    option.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Description = "Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { new OpenApiSecurityScheme { Reference = new OpenApiReference{ Type=ReferenceType.SecurityScheme,Id = JwtBearerDefaults.AuthenticationScheme} },Array.Empty<string>() }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(option =>
//    {
//        option.DocumentTitle = "Morin API v1.0";
//        option.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Morin API v1.0");
//    });
//}

if (sectionSite["NeedSwagger"] == "1")
{
    app.UseSwagger();
    app.UseSwaggerUI(option =>
    {
        option.DocumentTitle = "Morin API v1.0";
        option.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Morin API v1.0");
    });
}

app.UseRouting();

app.UseCors("MrCors");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        context.Response.Redirect("http://huanghunxiao.com");
        await Task.CompletedTask;
    });
});

app.Run();
