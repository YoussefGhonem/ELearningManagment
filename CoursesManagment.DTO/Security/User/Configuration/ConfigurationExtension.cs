using Microsoft.Extensions.Configuration;

namespace CoursesManagment.DTO.Security.User.Configuration;

public static class ConfigurationExtension
{
    public static JwtConfig GetJwtConfig(this IConfiguration configuration)
    {
        var config = configuration.GetSection("Jwt").Get<JwtConfig>();
        if (config is null)
        {
            throw new Exception("Missing 'Jwt' configuration section from the appsettings.");
        }

        return config;
    }
}