using CoursesManagment.Core.Interfaces;
using System.Collections.Generic;

namespace CoursesManagment.Core.Common
{
    public class ResponseDTO : IResponseDTO
    {
        public ResponseDTO()
        {
            IsPassed = false;
            Message = "";
            Errors = new List<string>();
        }
        public bool IsPassed { get; set; }
        public List<string> Errors { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }

        public void Copy(ResponseDTO x)
        {
            IsPassed = x.IsPassed;
            Message = x.Message;
            Data = x.Data;
            Errors = x.Errors;
        }
    }
}
