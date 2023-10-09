using System.Diagnostics.CodeAnalysis;

namespace TeacherPublishingDemo
{
    [ExcludeFromCodeCoverage]
    public class Course
    {
        public string Teacher { get; set; }
        public string Domain { get; set; }
        public string Module { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
