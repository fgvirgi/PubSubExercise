namespace TeacherPublisherAPI.Domain.Exceptions
{
    [Serializable]
    public class TranslationFailedException : Exception
    {
        public TranslationFailedException(string message)
            : base($"Translation failed: {message}.")
        {
        }
    }
}
