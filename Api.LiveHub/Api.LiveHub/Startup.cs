using Api.LiveHub.AutoMapper;
using Api.LiveHub.Filters;
using Api.LiveHub.Infrastrue.DataContext;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.RegisterServices;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Api.LiveHub
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
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, // verify that the key used to sign the incoming token is part of a list of trusted keys
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"])), // appsettings.json文件中定义的JWT Key

                    ValidateIssuer = true, // validate the server
                    ValidIssuer = Configuration["JWT:Issuer"], // 发行人

                    ValidateAudience = true, // ensure that the recipient of the token is authorized to receive it
                    ValidAudience = Configuration["JWT:Audience"], // 订阅人

                    ValidateLifetime = true, // check that the token is not expired and that the signing key of the issuer is valid
                    // 注意这是缓冲过期时间，总的有效时间等于这个时间加上jwt的过期时间，如果不配置，默认是5分钟
                    ClockSkew = TimeSpan.FromHours(24*90),
                    RequireExpirationTime = true
                };
            });
            services.AddMvc(options =>
            {
                // 权限过滤器
                options.Filters.Add(new WeiXinAuthorizeFilter());
                // 异常过滤器
                options.Filters.Add(new WeiXinExceptionFilter());
            });
            services.AddDbContext<BusinessDbContext>(opt =>
            {
                opt.UseLazyLoadingProxies().UseMySql(Configuration.GetConnectionString("DefaultConnection"));
            });
            //automapper服务
            services.AddAutoMapper(typeof(DomainToViewModelMappingProfile));
            services.AddControllers();
            //services.AddMvc();
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMemoryCache();//使用本地缓存必须添加
            //services.AddSession();//使用Session
            services.AddSwaggerGen(c =>
            {
                //c.SwaggerDoc("v1", new Info
                //{
                //    Version = "v0.1.0",
                //    Title = "接口文档 API",
                //    Description = "LiveHubAPI",
                //    TermsOfService = "None",
                //});
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "LiveHub API",
                    Version = "v1",
                    Description = "LiveHubAPI接口文档",
                    Contact = new OpenApiContact
                    {
                        Name = "codexp",
                        Url = new Uri("https://codexp.club"),
                    }
                });
                //为 Swagger JSON and UI设置xml文档注释路径
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                //var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
                //Logger.Info(basePath);
                //var xmlPath = Path.Combine(basePath, "LiveHub.Api.xml");//配置的xml文件名
                //c.IncludeXmlComments(xmlPath, true);//controller的注释

            });
            services.AddSenparcGlobalServices(Configuration)//Senparc.CO2NET 全局注册
                    .AddSenparcWeixinServices(Configuration);//Senparc.Weixin 注册
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<SenparcSetting> senparcSetting, 
            IOptions<SenparcWeixinSetting> senparcWeixinSetting)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
                c.RoutePrefix = "swagger";
            });
            #endregion
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //使用 SignalR
            //app.UseSignalR(routes =>
            //{
            //    routes.MapHub<SenparcHub>("/senparcHub");
            //});

            // 启动 CO2NET 全局注册，必须！
            IRegisterService register = RegisterService.Start(env, senparcSetting.Value).UseSenparcGlobal();


            //开始注册微信信息，必须！
            register.UseSenparcWeixin(senparcWeixinSetting.Value, senparcSetting.Value);

            /* 微信配置结束 */


        }

    }
}
