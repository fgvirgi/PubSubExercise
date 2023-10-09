using StudentSubscriber.Entities;

namespace StudentSubscriber.Application
{
    public interface IStudentRepository
    {
        public Task<List<DomainInfo>> GetDomainsToWatch();
    }
}
