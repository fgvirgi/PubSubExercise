using StudentSubscriber.Application;
using StudentSubscriber.Entities;

namespace StudentSubscriber.Services
{
    public class StudentsHostedService : BackgroundService
    {
        private readonly IStudentRepository _repository;
        private readonly ILogger<StudentsHostedService> _logger;

        public StudentsHostedService(IServiceProvider services,
            IStudentRepository repository,
            ILogger<StudentsHostedService> logger)
        {
            Services = services;
            _repository = repository;
            _logger = logger;
        }

        public IServiceProvider Services { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Exemplify subscription from multiple students.");

            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 3; i++)
            {
                var domainsToWatch = await _repository.GetDomainsToWatch();
                foreach (var domain in domainsToWatch)
                {
                    tasks.Add(SubscribeToDomain(i, domain, stoppingToken));
                }
            }

            await Task.WhenAll(tasks);

            _logger.LogInformation("Vacation is starter, stop listening ...");
            await StopAsync(stoppingToken);
        }

        private async Task SubscribeToDomain(int studentId, DomainInfo domainInfo, CancellationToken stoppingToken)
        {
            using (var scope = Services.CreateScope())
            {
                var service = scope.ServiceProvider
                                .GetRequiredService<IDomainSubscriberService>();

                await service.ListenToTeacherChannel(studentId, domainInfo, stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
