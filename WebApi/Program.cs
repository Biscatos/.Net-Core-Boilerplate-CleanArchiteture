using Core.Application;
using Infrastruture.Identity;
using Infrastruture.Persistence;
using Infrastruture.Shared;
using Microsoft.OpenApi.Models;
using WebApi.Extensions;


internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        builder.Services.AddPersistenceServices(builder.Configuration);
        builder.Services.AddApplicationServices();
        builder.Services.AddSharedServices(builder.Configuration);
        builder.Services.AddIdentityServices(builder.Configuration);

        // adding swagger configs
        builder.Services.AddSwaggerConfigurations();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();


        builder.Services.AddApiVersioning(o =>
        {
            o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            o.AssumeDefaultVersionWhenUnspecified = true;
            o.ReportApiVersions = true;
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();
         
        app.MapControllers();

        app.Run();
    }
}