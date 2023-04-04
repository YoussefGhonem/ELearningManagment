using CoursesManagment.Data;
using Microsoft.EntityFrameworkCore;

namespace CoursesManagment.Geteway.Extensions
{
    public static class DBConfigurationExtension
    {
        public static IServiceCollection AddDBConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetSection("StoreOptions").GetValue<string>("ConnectionString")));
            return services;
        }
    }
}
    