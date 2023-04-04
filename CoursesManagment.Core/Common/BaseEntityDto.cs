using System;

namespace CoursesManagment.Core.Common
{
    public class BaseEntityDto
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public string Creator { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public string Updater { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
