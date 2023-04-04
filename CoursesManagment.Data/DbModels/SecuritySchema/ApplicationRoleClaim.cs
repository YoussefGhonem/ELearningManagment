using Microsoft.AspNetCore.Identity;

namespace CoursesManagment.Data.DbModels.SecuritySchema
{
    public class ApplicationRoleClaim : IdentityRoleClaim<Guid>
    {
        public virtual ApplicationRole Role { get; set; }
    }
}
