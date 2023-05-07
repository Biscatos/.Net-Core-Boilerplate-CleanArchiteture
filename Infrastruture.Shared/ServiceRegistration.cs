using Core.Application.Interfaces.Services.System;
using Core.Application.SystemSettings;
using Infrastruture.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastruture.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedServices(this IServiceCollection service, IConfiguration configuration)
        {

            service.Configure<MinIoSetting>(o => configuration.GetSection("MinIO"));

            service.AddTransient<ILocalFilesService, LocalFilesService>();
            service.AddTransient<IMinIoService, MinIoService>();
            service.AddTransient<IApplicationLogger, ApplicationLogger>();
            service.AddTransient<IEmailIntegrationService, EmailIntegrationService>();
        }
    }
}