using BusinessLayer;
using Quartz;
using ServiceLayer.Implementation;
using ServiceLayer.Interface;
using static Quartz.Logging.OperationName;
using System.Reactive.Concurrency;
using IScheduler = ServiceLayer.Interface.IScheduler;

namespace ClientWebApi
{
    public static class ServiceExtension
    {
        public static void DIScope(this IServiceCollection service)
        {
            service.AddScoped<IClient, ClientImpl>();
            service.AddScoped<ClientBLL>();

            service.AddScoped<ISalary, SalaryImpl>();
            service.AddScoped<SalaryBLL>();

            service.AddScoped<ILogin, LoginImpl>();
            service.AddScoped<LoginBLL>();

            service.AddScoped<IExcel, ExcelImpl>();
            service.AddScoped<ExcelBLL>();

            service.AddScoped<IAttendance, AttendanceImpl>();
            service.AddScoped<AttendanceBLL>();

            service.AddScoped<IScheduler, SchedulerImpl>();
            service.AddScoped<SchedulerBLL>();

            service.AddQuartz(options =>
            {
                options.UseMicrosoftDependencyInjectionJobFactory();

                var jobKey = JobKey.Create(nameof(SchedulerBLL));

                options.AddJob<SchedulerBLL>(jobKey)
                .AddTrigger(trigger => trigger.ForJob(jobKey)
                  //.WithSimpleSchedule(schedule => schedule.WithIntervalInMinutes(2).RepeatForever())
                  //.WithSchedule(CronScheduleBuilder.MonthlyOnDayAndHourAndMinute(0 0 3 L * ?)) //L-1
                  //  .WithCronSchedule("0 44 17 11 * ?")
                  .WithCronSchedule("0 56 17 11 * ?")
                );

                options.AddJob<SchedulerBLL>(jobKey)
             .AddTrigger(trigger => trigger.ForJob(jobKey)
               .WithCronSchedule("0 57 17 11 * ?")
             );

            });


            service.AddQuartzHostedService();
        }
    }
}
