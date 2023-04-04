using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CoursesManagment.Core.CurrentUser
{
    public static class CurrentUserInjection
    {
        public static IApplicationBuilder UseCurrentUser(this IApplicationBuilder app)
        {
            CurrentUser.InitializeHttpContextAccessor(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            return app;
        }
    }
}
