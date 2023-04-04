using CoursesManagment.Core.Common;
using CoursesManagment.Core.Enums;

namespace CoursesManagment.DTO.Security.User
{
    public class UserFilterDto : BaseFilterDto
    {
        public string? SearchWord { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; } // Active, NotActive
        public ApplicationRolesEnum? Role { get; set; }
    }
}
