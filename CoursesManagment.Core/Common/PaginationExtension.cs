namespace CoursesManagment.Core.Common;

public static class PaginationExtension
{
    public static (List<T> list, int total) Paginate<T>(this IQueryable<T> query, int? pageSize, int? pageNumber)
    {
        const int maxPageSize = 20;
        var paginatedList = new List<T>();

        if (!pageSize.HasValue && !pageNumber.HasValue)
        {
            paginatedList = query.Take(maxPageSize).ToList();
            return (paginatedList.ToList(), query.Count());
        }

        var pageIndex = pageNumber!.Value - 1;
        paginatedList = query.Skip(pageIndex * pageSize!.Value).Take(pageSize.Value).ToList();
        return (paginatedList, query.Count());
    }
}