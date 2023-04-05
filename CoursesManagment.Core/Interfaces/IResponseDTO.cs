using CoursesManagment.Core.Common;

namespace CoursesManagment.Core.Interfaces
{
    public interface IResponseDTO
    {
        bool IsPassed { get; set; }
        string Message { get; set; }
        List<ResponseErrorDto> Errors { get; set; }
        dynamic Data { get; set; }
    }
}
