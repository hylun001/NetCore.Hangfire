using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCore.Hangfire.Interface;
using NetCore.Hangfire.Service;
using System;

namespace NetCore.Hangfire
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
            services.AddSingleton<ICustomService, CustomService>();
            services.AddHostedService<BackgroundJobService>();
            services.AddHangfire((x) =>
            {
                x.UseStorage(new MemoryStorage());
            });
            services.AddHangfireServer((options) =>
            {
                options.SchedulePollingInterval = TimeSpan.FromSeconds(15);
                options.WorkerCount = Environment.ProcessorCount * 5; //并发任务数
                options.ServerName = "My_Handfire";//服务器名称
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                AppPath = "#",//返回时跳转的地址
                DisplayStorageConnectionString = true,//是否显示数据库连接信息
                IsReadOnlyFunc = Context =>
                {
                    return false;
                },
                DashboardTitle="MyHangfire",
                Authorization = new[] {  
                    new BasicAuthAuthorizationFilter(
                    new BasicAuthAuthorizationFilterOptions
                 {
                     RequireSsl = false,//是否启用ssl验证，即https
                     SslRedirect = false,// 是否将所有非SSL请求重定向到SSL URL
                     LoginCaseSensitive = true,//登录检查是否区分大小写
                     Users = new []
                     {
                        new BasicAuthAuthorizationUser
                         {
                             Login = "test",
                             PasswordClear = "123456"
                         }
                    }
                 })
                }
            });
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
