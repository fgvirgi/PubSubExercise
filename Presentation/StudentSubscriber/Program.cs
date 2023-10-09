using PubSub.Domain.Configuration;
using PubSub.Domain.Interfaces;
using PubSub.Messaging.RabbitMQ;
using StudentSubscriber.Application;
using StudentSubscriber.Persistence;
using StudentSubscriber.Services;


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<StudentsHostedService>();

        var config = context.Configuration;
        services.Configure<MessageClientOptions>(
            config.GetSection(key: nameof(MessageClientOptions)));
        services.AddScoped<IMessageClient, MessageClient>();

        services.AddSingleton<IStudentRepository, StudentRepository>();
        services.AddScoped<IDomainSubscriberService, DomainSubscriberService>();
    })
    .Build();

Console.Title = "Student subscribers demo";

await host.RunAsync();

Console.ReadKey();
