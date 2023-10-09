using PubSub.Domain.Interfaces;
using TeacherPublisherAPI.Domain.Entities;
using TeacherPublisherAPI.Domain.Exceptions;

namespace TeacherPublisherAPI.Services
{
    public class TeacherPublisherService : ITeacherPublisherService
    {
        private readonly IMessageClient _publisher;
        private readonly ICourseTranslator _translator;
        private readonly ILogger _logger;

        public TeacherPublisherService(IMessageClient publisher, ICourseTranslator translator, ILogger<TeacherPublisherService> logger)
        {
            _publisher = publisher;
            _translator = translator;
            _logger = logger;
        }

        public async Task PublishCourse(Course course, string languageCode, CancellationToken token)
        {
            if (course is null) return;

            try
            {
                var translatedCourse = await _translator.Translate(course, languageCode, token);
                if (translatedCourse is null)
                {
                    throw new TranslationFailedException($"{course.Title}");
                }

                var channel = GetTeacherChannel(course);
                await _publisher.Publish(channel, translatedCourse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while publishing the course '{course.Module}:{course.Title}' by {course.Teacher}.");
                throw;
            }
        }

        private ITeacherChannel GetTeacherChannel(Course course)
        {
            return course;
        }
    }
}
