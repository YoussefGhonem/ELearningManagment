using CoursesManagment.Core.Common;
using FluentValidation.Results;

namespace CoursesManagment.Core.Extensions
{
    public static class ErrorMsgExtension
    {
        public static ResponseErrorDto ErrorMsg(this string error)
        {
            return new ResponseErrorDto
            {
                Title = "Error",
                Description = error,
                Code = null,
            };
        }

        public static List<ResponseErrorDto> ErrorMsg(this ValidationResult validationResult)
        {
            return validationResult.Errors.Select(e => e.ErrorMessage).Distinct().Select(x => x.ErrorMsg()).ToList();
        }
    }
}