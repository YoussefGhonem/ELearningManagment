namespace CoursesManagment.Core.Common
{
    public class ResponseErrorDto
    {
        public ResponseErrorDto()
        {
            Title = "Error";
        }

        public string Title { get; set; } // user friendly title of error message to be provided by user
        public string Description { get; set; } // user friendly description of the error message to be provided by user
        public string Code { get; set; } // System generated to uniquely identify every error message
    }
}