using System.Diagnostics.CodeAnalysis;

namespace PubSub.Domain.Configuration
{
    [ExcludeFromCodeCoverage]
    public class MessageClientOptions
    {
        public string HostName { get; set; } = "localhost";
    }
}
