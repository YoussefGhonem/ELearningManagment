namespace CoursesManagment.Core.Interfaces
{
    public interface ILookupEntity
    {
        int Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
    }
}
