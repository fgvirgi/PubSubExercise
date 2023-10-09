using System.Diagnostics.CodeAnalysis;
using PubSub.Domain.Interfaces;

namespace TeacherPublisherAPI.Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class Course : ICourseContent, ITeacherChannel
    {
        public string Teacher { get; set; }
        public string Domain { get; set; }
        public string Module { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
