namespace CoursesManagment.Core.Common
{
    public class BaseFilterDto
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public bool ApplySort { get; set; }
        public bool IsAscending { get; set; }
    }
}
