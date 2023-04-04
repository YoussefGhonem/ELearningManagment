using System;

namespace CoursesManagment.Core.Common
{
    public class DefaultIdAuditableEntityDto
    {
        public Guid Id { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
    }
}
