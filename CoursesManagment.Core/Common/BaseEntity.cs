namespace CoursesManagment.Core.Common
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public BaseEntity()
        {
            IsActive = true;
            IsDeleted = false;
            CreatedOn = DateTime.Now;
        }
    }
}
