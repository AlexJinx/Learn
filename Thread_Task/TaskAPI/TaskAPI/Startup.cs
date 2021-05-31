using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TaskAPI
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0", new OpenApiInfo { Title = "����С���� API", Version = "v1.0" });
                //c.SchemaFilter<SwaggerIgnoreExFilter>();
                //c.DocumentFilter<SwaggerIgnoreApiFilter>("/apiadmin/");
                //c.DocumentFilter<SwaggerIgnoreApiFilter>("/api/");

                //�����ĵ��ӿ�ע��
                //string basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location); //��ȡӦ������Ŀ¼
                //string xmlPath = Path.Combine(basePath, "CQGJZT.CQHL.Web.xml");
                //c.IncludeXmlComments(xmlPath);

                //������Ȩ
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = "�ı���������JWT���ɵ�Token����ʽΪ��Bearer token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    BearerFormat = "JWT",
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = JwtBearerDefaults.AuthenticationScheme
                                }
                            }, new string[]{ }
                        }
                    }); ;
            });
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
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "����С���� API v1.0";
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "����С���� API v1.0");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
