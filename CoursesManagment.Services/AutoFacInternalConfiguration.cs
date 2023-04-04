using CoursesManagment.Core.Common;
using CoursesManagment.Core.Interfaces;
using CoursesManagment.Services.Security.Account;
using Microsoft.Extensions.DependencyInjection;

namespace CoursesManagment.Services
{
    public static class AutoFacInternalConfiguration
    {
        public static IServiceCollection AddServicesApplication(this IServiceCollection services)
        {
            services.AddScoped<IResponseDTO, ResponseDTO>();
            services.AddScoped<IAccountService, AccountService>();

            return services;
        }
    }
}
