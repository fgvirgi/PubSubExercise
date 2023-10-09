using PubSub.Domain.Configuration;
using PubSub.Domain.Interfaces;
using PubSub.Messaging.RabbitMQ;
using System.Diagnostics.CodeAnalysis;
using TeacherPublisherAPI.Services;

namespace TeacherPublisherAPI
{
    internal static class DependencyInjection
    {
        [ExcludeFromCodeCoverage]
        internal static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            var config = builder.Configuration;
            var services = builder.Services;

            services.Configure<MessageClientOptions>(
                config.GetSection(key: nameof(MessageClientOptions)));

            services.AddSingleton<ICourseTranslator, CourseTranslator>();
            services.AddScoped<IMessageClient, MessageClient>();
            services.AddScoped<ITeacherPublisherService, TeacherPublisherService>();
            
            return builder;
        }
    }
}
