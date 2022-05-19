using Microsoft.OpenApi.Models;
using RC.CA.WebApi.Utilities;

namespace RC.CA.WebApi;

public static class SetupSwaggerService
{
    public static IServiceCollection SetupSwaggerServices(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer 123456789...'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });

            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "CA Management API",
            });
            //[Support for csv file]
            c.OperationFilter<FileResultContentTypeOperationFilter>();
        });
        return services;
    }
}
