using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CoursesManagment.Geteway.Extensions;

public static class ConfigurationExtension
{

    public static SwaggerGenOptions AddAuthorizationWithJwt(this SwaggerGenOptions options)
    {
        options.AddAuthorizationHeader();
        options.AddAuthorizationRequirement();
        return options;
    }

    public static SwaggerGenOptions AddAuthorizationHeader(this SwaggerGenOptions options)
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter 'Bearer' [space] and then your token in the text input."
        });
        return options;
    }
    public static SwaggerGenOptions AddAuthorizationRequirement(this SwaggerGenOptions options)
    {
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
        return options;
    }
}