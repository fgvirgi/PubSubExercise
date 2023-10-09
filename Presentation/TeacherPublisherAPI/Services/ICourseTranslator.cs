using PubSub.Domain.Interfaces;

namespace TeacherPublisherAPI.Services
{
    public interface ICourseTranslator
    {
        public Task<ICourseContent?> Translate(ICourseContent course, string targetLanguageCode, CancellationToken token);
    }
}
