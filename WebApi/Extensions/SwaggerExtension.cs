using Microsoft.OpenApi.Models;

namespace WebApi.Extensions
{
    public static class SwaggerExtension
    {

        public static void AddSwaggerConfigurations(this IServiceCollection services)
        {

            services.AddSwaggerGen(o =>
            {

                o.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Boilerplate API",
                    Description = "This is the API for the Wizzzard Project",
                    Contact = new OpenApiContact
                    {
                        Email = "xuxunguinho@outlook.com",
                        Name = "xuxunguinho INC",
                        Url = new Uri("https://www.xuxunguinho.com")

                    }
                });

                o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
                });

                o.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },

                });
            });

        }
    }
}
