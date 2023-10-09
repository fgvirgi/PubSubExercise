namespace PubSub.Domain.Interfaces
{
    public interface ICourseContent
    {
        public string Module { get; }
        public string Title { get; }
        public string Content { get; }
    }
}
