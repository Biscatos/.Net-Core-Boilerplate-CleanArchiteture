using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace Core.Application
{
    public static class ServiceRegistration
    {

        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());


        }
    }
}