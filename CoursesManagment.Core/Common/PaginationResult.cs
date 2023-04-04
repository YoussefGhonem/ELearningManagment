namespace CoursesManagment.Core.Common;

public class PaginationResult<T>
{
    public int Total { get; }
    public List<T> Data { get; }

    public PaginationResult(List<T>? data, int total)
    {
        Data = data ?? new List<T>();
        Total = total;
    }
}