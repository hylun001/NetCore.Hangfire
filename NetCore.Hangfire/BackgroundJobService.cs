using Hangfire;
using Microsoft.Extensions.Hosting;
using NetCore.Hangfire.Interface;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Hangfire
{
    public class BackgroundJobService : BackgroundService
    {
        private readonly ICustomService _customerService;

        public BackgroundJobService(ICustomService customService)
        {
            _customerService = customService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            #region 循环任务
            RecurringJob.AddOrUpdate("seconds", () => Console.WriteLine("我是秒!"), "*/1 * * * * *");
            RecurringJob.AddOrUpdate("job1", () => _customerService.SayMessage("今天是个好日子!"), Cron.Minutely());
            RecurringJob.AddOrUpdate("job2", () => Job2(), Cron.Minutely());
            RecurringJob.AddOrUpdate("job3", () => Job3(), Cron.Minutely());
            RecurringJob.AddOrUpdate("job4", () => Job4(), Cron.Minutely());
            #endregion 循环任务

            #region 延时任务

            BackgroundJob.Schedule(() => Console.WriteLine("Delayed!"), TimeSpan.FromSeconds(2.0));

            #endregion 延时任务

            #region 队列任务
            BackgroundJob.Enqueue(() => Console.WriteLine("Fire-and-forget!"));
            #endregion 队列任务

            return Task.CompletedTask;
        }

        public void Job1()
        {
            Thread.Sleep(1000);
            Console.WriteLine("Job1 say hello");
        }

        public void Job2()
        {
            Thread.Sleep(1000);
            Console.WriteLine("Job2 say hello");
        }

        public void Job3()
        {
            Thread.Sleep(1000);
            Console.WriteLine("Job3 say hello");
        }

        public void Job4()
        {
            Thread.Sleep(1000);
            Console.WriteLine("Job4 say hello");
        }
    }
}
