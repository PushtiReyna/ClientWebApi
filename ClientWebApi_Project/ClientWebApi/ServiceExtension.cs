using BusinessLayer;
using ServiceLayer.Implementation;
using ServiceLayer.Interface;

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
        }
    }
}
