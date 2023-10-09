using Microsoft.Extensions.Logging;
using PubSub.Domain.Interfaces;
using TeacherPublisherAPI.Domain.Entities;
using TeacherPublisherAPI.Services;

namespace TeacherPublisherAPITests.Services
{
    public class TeacherPublisherServiceTests
    {
        private readonly Mock<ICourseTranslator> _translatorMock;
        private readonly Mock<IMessageClient> _messageClientMock;
        private readonly Mock<ILogger<TeacherPublisherService>> _loggerMock;
        public TeacherPublisherServiceTests()
        {
            _translatorMock = new Mock<ICourseTranslator>();
            _messageClientMock = new Mock<IMessageClient>();
            _loggerMock = new Mock<ILogger<TeacherPublisherService>>();
        }

        [Fact]
        public async Task PublishCourse_ValidCourse_PublishesTranslatedCourse()
        {
            // Arrange
            var course = new Course
            {
                Module = "Sample Module",
                Title = "Sample Title",
                Teacher = "Sample Teacher"
            };

            var languageCode = "en";
            var cancellationToken = CancellationToken.None;

            _translatorMock
                .Setup(translator => translator.Translate(course, languageCode, cancellationToken))
                .ReturnsAsync(new Course
                {
                    Module = course.Module,
                    Title = $"Translated Title in {languageCode}",
                    Teacher = course.Teacher
                });

            var teacherPublisherService = new TeacherPublisherService(
                _messageClientMock.Object,
                _translatorMock.Object,
                _loggerMock.Object);

            // Act
            await teacherPublisherService.PublishCourse(course, languageCode, cancellationToken);

            // Assert
            // Verify that the translator.Translate method was called with the correct arguments
            _translatorMock.Verify(
                translator => translator.Translate(course, languageCode, cancellationToken),
                Times.Once);

            // Verify that the messageClient.Publish method was called with the translated course
            _messageClientMock.Verify(
                messageClient => messageClient.Publish(It.IsAny<ITeacherChannel>(), It.IsAny<Course>()),
                Times.Once);

            // Ensure that the logger was not called for errors
            _loggerMock.Verify(logger => logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                    It.Is<EventId>(eventId => eventId.Id == 0),
                      It.Is<It.IsAnyType>((@object, @type) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Never);
        }

        [Fact]
        public async Task PublishCourse_TranslateThrowsError_LogsErrorAndThrows()
        {
            // Arrange
            var course = new Course();
            var languageCode = "en";
            var cancellationToken = CancellationToken.None;

            _translatorMock
                .Setup(translator => translator.Translate(course, languageCode, cancellationToken))
                .ThrowsAsync(new Exception("Translation error"));

            var teacherPublisherService = new TeacherPublisherService(
                _messageClientMock.Object,
                _translatorMock.Object,
                _loggerMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await teacherPublisherService.PublishCourse(course, languageCode, cancellationToken);
            });

            // Verify that the translator.Translate method was called with the correct arguments
            _translatorMock.Verify(
                translator => translator.Translate(course, languageCode, cancellationToken),
                Times.Once);

            // Ensure that the logger was called for errors
            _loggerMock.Verify(logger => logger.Log(
                    It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
                    It.Is<EventId>(eventId => eventId.Id == 0),
                    It.Is<It.IsAnyType>((@object, @type) => true),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
