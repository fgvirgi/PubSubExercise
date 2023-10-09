using StudentSubscriber.Application;
using StudentSubscriber.Entities;

namespace StudentSubscriber.Persistence
{
    //Remark: this is just a dummy repository
    public class StudentRepository : IStudentRepository
    {
        private static readonly string[] Domains = { "Computing and maths", "Philosophy" };
        private static readonly IEnumerable<int> TeacherIds = Enumerable.Range(1, 10);
        
        public async Task<List<DomainInfo>> GetDomainsToWatch()
        {
            var watchTeacherIds = TeacherIds.OrderBy(x => Random.Shared.Next()).Take(3);
            List<DomainInfo> domainInfos = new();
            foreach (var teacherId in watchTeacherIds)
            {
                var domain = GetTeacherDomain(teacherId);
                var domainInfo = new DomainInfo
                {
                    Teacher = $"Teacher_{teacherId}",
                    Domain = domain
                };
                domainInfos.Add(domainInfo);
            }
            
            return await Task.FromResult(domainInfos);
        }

        private static string GetTeacherDomain(int teacherId)
        {
            return Domains[teacherId % Domains.Length];
        }
    }
}
