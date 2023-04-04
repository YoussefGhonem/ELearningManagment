using Microsoft.AspNetCore.Identity;

namespace CoursesManagment.Data.DbModels.SecuritySchema
{
    public class ApplicationUserClaim : IdentityUserClaim<Guid>
    {
        public virtual ApplicationUser User { get; set; }
    }
}
