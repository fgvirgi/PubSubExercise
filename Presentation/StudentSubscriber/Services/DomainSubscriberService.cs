using PubSub.Domain.Interfaces;
using StudentSubscriber.Application;

namespace StudentSubscriber.Services
{
    public class DomainSubscriberService : IDomainSubscriberService
    {
        private int _studentId;
        private readonly IMessageClient _publisher;
        private readonly ILogger _logger;

        public DomainSubscriberService(IMessageClient publisher, ILogger<DomainSubscriberService> logger)
        {
            _publisher = publisher;
            _logger = logger;
        }

        public async Task ListenToTeacherChannel(int studentId, ITeacherChannel channel, CancellationToken stoppingToken)
        {
            _studentId = studentId;
            _logger.LogInformation($"Student_{_studentId}>Subscribe to channel {channel.Teacher}: {channel.Domain}");

            _publisher.Subscribe(channel, ConsumeMessages);

            await Task.Delay(100000, stoppingToken);

            _logger.LogInformation($"Student_{_studentId}>unsubscribe from channel {channel.Teacher}: {channel.Domain}");
            _publisher.Unsubscribe();
        }

        private async Task ConsumeMessages(ITeacherChannel channel, ICourseContent courseContent)
        {
            _logger.LogInformation($">Student_{_studentId}>Received ... {channel.Teacher}: {channel.Domain} ... '{courseContent.Title}'");

            await Task.Delay(1);
        }
    }
}
