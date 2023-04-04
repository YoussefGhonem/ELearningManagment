using Microsoft.AspNetCore.Identity;

namespace CoursesManagment.Data.DbModels.SecuritySchema
{
    public class ApplicationUserLogin : IdentityUserLogin<Guid>
    {
        public virtual ApplicationUser User { get; set; }
    }
}
