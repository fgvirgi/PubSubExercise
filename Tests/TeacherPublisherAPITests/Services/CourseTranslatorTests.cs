using TeacherPublisherAPI.Domain.Entities;
using TeacherPublisherAPI.Services;

namespace TeacherPublisherAPITests.Services
{
    public class CourseTranslatorTests
    {
        [Fact]
        public async Task Translate_ShouldTranslateCourseContent()
        {
            // Arrange
            var course = new Course
            {
                Title = "Sample Title",
                Content = "Sample Content"
            };

            var targetLanguageCode = "fr"; // French, for example
            var cancellationToken = CancellationToken.None;

            var translator = new CourseTranslator();

            // Act
            var translatedCourse = await translator.Translate(course, targetLanguageCode, cancellationToken);

            // Assert
            Assert.NotNull(translatedCourse);
            Assert.IsType<Course>(translatedCourse);

            // Check if the title and content are properly marked with the target language code
            Assert.StartsWith($"||{targetLanguageCode.ToUpper()}||", translatedCourse.Title);
            Assert.StartsWith($"||{targetLanguageCode.ToUpper()}||", translatedCourse.Content);

            // Ensure that the original course object is not modified
            Assert.Equal("Sample Title", course.Title);
            Assert.Equal("Sample Content", course.Content);
        }
    }
}
