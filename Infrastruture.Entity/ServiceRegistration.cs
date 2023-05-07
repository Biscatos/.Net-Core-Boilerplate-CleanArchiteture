using Core.Application.Interfaces.Services.System;
using Core.Application.Wrappers;
using Core.Domain.Entities.System;
using Infrastruture.Identity.Context;
using Infrastruture.Identity.Services;
using Infrastruture.Identity.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;


namespace Infrastruture.Identity
{
    public static class ServiceRegistration
    {
        public static void AddIdentityServices(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<IdentityContext>(o => o.UseSqlServer(configuration.GetConnectionString("default"),
                builderOp => builderOp.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)));

            service.AddIdentity<ApplicationUser, IdentityRole>()
              .AddEntityFrameworkStores<IdentityContext>()
                  .AddDefaultTokenProviders();


            service.Configure<JWTSettings>(options => configuration.GetSection("JWTSettings").Bind(options));


            #region Services
            service.AddTransient<IAccountService, AccountService>();
            #endregion
            service.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Path.GetTempPath()));
            #region AddingJWToken
            service.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {

                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JWTSettings:Issuer"],
                    ValidAudience = configuration["JWTSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))

                };

                o.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = f =>
                    {
                        f.NoResult();
                        f.Response.StatusCode = 500;
                        f.Response.ContentType = "application/json";
                        return f.Response.WriteAsync(f.Exception.ToString());
                    },
                    OnChallenge = context =>
                    {

                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new Response<string>("You are not Authorized"));
                        return context.Response.WriteAsync(result);
                    },

                };

            });
            #endregion


        }
    }
}