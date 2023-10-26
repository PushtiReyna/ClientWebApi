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
        }
    }
}
