using System.Collections.Generic;

namespace CoursesManagment.Core.Interfaces
{
    public interface IResponseDTO
    {
        bool IsPassed { get; set; }
        string Message { get; set; }
        List<string> Errors { get; set; }
        dynamic Data { get; set; }
    }
}
