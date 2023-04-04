namespace CoursesManagment.Geteway.Extensions
{
    public static class ControllerConfigurationExtension
    {
        public static IServiceCollection AddControllerConfiguration(this IServiceCollection services)
        {
            services.AddControllers();
            return services;
        }

        public static WebApplication UseControllerConfiguration(this WebApplication app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return app;
        }
    }
}
