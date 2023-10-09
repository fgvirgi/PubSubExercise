using System.Text.Json;
using PubSub.Domain.Interfaces;
using TeacherPublisherAPI.Domain.Entities;

namespace TeacherPublisherAPI.Services
{
    public class CourseTranslator : ICourseTranslator
    {
        public async Task<ICourseContent?> Translate(ICourseContent course, string targetLanguageCode, CancellationToken token)
        {
            //Remark: this is just a dummy implementation to translate the course
            string jsonString = JsonSerializer.Serialize(course);
            var courseOut = JsonSerializer.Deserialize<Course>(jsonString);

            var mark = $"||{targetLanguageCode.ToUpper()}||";
            courseOut.Title = $"{mark} {courseOut.Title}";
            courseOut.Content = $"{mark} {courseOut.Content}";

            return await Task.FromResult(courseOut);
        }
    }
}
