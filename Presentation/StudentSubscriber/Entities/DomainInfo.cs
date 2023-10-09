using PubSub.Domain.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace StudentSubscriber.Entities
{
    [ExcludeFromCodeCoverage]
    public class DomainInfo : ITeacherChannel
    {
        public string Teacher { get; set; }
        public string Domain { get; set; }
    }
}
