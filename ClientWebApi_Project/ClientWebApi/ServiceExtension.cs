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
                  .WithSimpleSchedule(schedule => schedule.WithIntervalInMinutes(4).RepeatForever())
                  //.WithSchedule(CronScheduleBuilder.MonthlyOnDayAndHourAndMinute(0 0 3 L * ?)) //L-1
                  //  .WithCronSchedule("0 0 0 1 * ?")
                 // .WithCronSchedule("0 22 15 12 * ?")
                );

                options.AddTrigger(trigger1 => trigger1.ForJob(jobKey)
                .WithCronSchedule("0 40 12 12 * ?")
                    //  .WithCronSchedule("0 0 20 L * ?")  0 15 10 L * ?
                    );

            });


            service.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        }
    }
}
