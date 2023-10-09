using TeacherPublisherAPI.Domain.Entities;

namespace TeacherPublisherAPI.Services
{
    public interface ITeacherPublisherService
    {
        public Task PublishCourse(Course course, string languageCode, CancellationToken token);
    }
}
