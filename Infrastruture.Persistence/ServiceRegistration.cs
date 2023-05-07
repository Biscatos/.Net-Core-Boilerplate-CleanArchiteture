using Core.Application.Interfaces.System;
using Infrastruture.Persistence.Contexts;
using Infrastruture.Persistence.Repositores.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastruture.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(configuration.GetConnectionString("default"),
                builderOp => builderOp.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));


            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));


        }
    }
}
