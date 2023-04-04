using CoursesManagment.Data.DbModels.SecuritySchema;
using CoursesManagment.DTO.Security.User;

namespace CoursesManagment.Services.Security.Account;

public static class ApplyFilterExtension
{
    public static IQueryable<ApplicationUser> Filter(this IQueryable<ApplicationUser> query, UserFilterDto filterDto)
    {
        // Filters
        if (filterDto != null)
        {
            if (!string.IsNullOrEmpty(filterDto.SearchWord))
            {
                query = query
                    .Where(x => x.FirstName.Trim().ToLower()
                    .Contains(filterDto.SearchWord.Trim().ToLower()) 
                    || x.LastName.Trim().ToLower().Contains(filterDto.SearchWord.Trim().ToLower())
                    || x.PhoneNumber.Trim().ToLower().Contains(filterDto.SearchWord.Trim().ToLower())
                    || x.Email.Trim().ToLower().Contains(filterDto.SearchWord.Trim().ToLower())
                    );
            }
            if (filterDto.Role != null)
            {
                query = query.Where(x => x.UserRoles.Any(r => r.Role.Name == filterDto.Role.ToString()));
            }
        }

        // Sort
         query = query.OrderByDescending(x => x.CreatedOn);
        return query;
    }
  
}