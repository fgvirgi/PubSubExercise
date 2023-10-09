using System.Diagnostics.CodeAnalysis;
using PubSub.Domain.Interfaces;

namespace PubSub.Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class TeacherChannel : ITeacherChannel
    {
        public string Teacher { get; set; }
        public string Domain { get; set; }
    }
}
