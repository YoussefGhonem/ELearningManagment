using CoursesManagment.Core.Common;
using CoursesManagment.DTO.Security.User;
using Mapster;

namespace CoursesManagment.Services.Security.Account;

public static class Mapper
{
    public static void ApplyMapping()
    {
        TypeAdapterConfig<Data.DbModels.SecuritySchema.ApplicationUser, UserDto>
            .NewConfig()
            .Map(dest => dest.Role, src => src.UserRoles.Select(userRole => userRole.Role.DisplayName).FirstOrDefault());

    }
}