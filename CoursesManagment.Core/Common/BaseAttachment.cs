namespace CoursesManagment.Core.Common
{
    public interface IBaseAttachment
    {
        public string? AttachmentName { get; }
        public string? AttachmentPath { get; }
        public string? AttachmentExtension { get; }
    }
}
