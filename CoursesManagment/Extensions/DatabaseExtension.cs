using CoursesManagment.Data;
using CoursesManagment.Data.DataContext;
using Microsoft.EntityFrameworkCore;

namespace CoursesManagment.Geteway.Extensions;

public static class DatabaseExtension
{
    public static async Task MigrateDatabase(this IServiceScope scope)
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    public static async Task SeedDatabase(this IServiceScope scope)
    {
        var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await DataSeedingIntilization.SeedDataAsync(appDbContext, scope.ServiceProvider);
    }
}
