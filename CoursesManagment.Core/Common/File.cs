namespace CoursesManagment.Core.Common
{
    public class File
    {
        public File()
        {
            //Content = new MemoryStream();
        }
        public string Name { get; set; }
        public string Extension { get; set; }
        public object Content { get; set; }
    }
}
