namespace CoursesManagment.Geteway.Extensions
{
    public static class SwaggerStartupExtention
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.AddAuthorizationWithJwt();
            });

            return services;
        }
        public static WebApplication UseBaseSwagger(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
            return app;
        }
    }
}
