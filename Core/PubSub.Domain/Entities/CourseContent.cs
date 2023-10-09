using System.Diagnostics.CodeAnalysis;
using PubSub.Domain.Interfaces;

namespace PubSub.Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class CourseContent : ICourseContent
    {
        public string Module { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
