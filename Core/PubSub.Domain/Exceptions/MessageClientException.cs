namespace PubSub.Domain.Exceptions
{
    [Serializable]
    public class MessageClientException : Exception
    {
        public MessageClientException(string message)
            : base($"Unable to create the message client: {message}")
        {
        }
    }
}
