namespace PubSub.Domain.Interfaces
{
    public delegate Task MessagePublishedCallback(ITeacherChannel channel, ICourseContent courseContent);

    public interface IMessageClient
    {
        public Task Publish(ITeacherChannel channel, ICourseContent course);
        public void Subscribe(ITeacherChannel channel, MessagePublishedCallback callback);
        public void Unsubscribe();
    }
}
