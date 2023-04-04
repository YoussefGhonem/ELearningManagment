using CoursesManagment.Core.Common;

namespace CoursesManagment.DTO.Security.User
{
    public class UserDto : DefaultIdAuditableEntityDto
    {
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public bool? IsActive { get; set; } // Active, NotActive, Locked
        public DateTime? BirthDate { get; set; }
        public string Role { get; set; }
    }
}
