using PubSub.Domain.Interfaces;

namespace StudentSubscriber.Application
{
    public interface IDomainSubscriberService
    {
        public Task ListenToTeacherChannel(int studentId, ITeacherChannel channel, CancellationToken stoppingToken);
    }
}
