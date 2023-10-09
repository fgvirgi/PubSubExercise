using Microsoft.Extensions.Options;
using PubSub.Domain.Configuration;
using PubSub.Domain.Entities;
using PubSub.Domain.Exceptions;
using PubSub.Domain.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;

namespace PubSub.Messaging.RabbitMQ
{
    public class MessageClient : IMessageClient, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private AsyncEventingBasicConsumer _consumer;
        private MessagePublishedCallback _callback;
        private string? _queueName;

        public MessageClient(IOptions<MessageClientOptions> options)
        {
            if (options is null) throw new ArgumentNullException(nameof(options));

            var config = options.Value;
            var factory = new ConnectionFactory
            {
                HostName = config.HostName,
                DispatchConsumersAsync = true
            };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
            }
            catch (Exception ex)
            {
                throw new MessageClientException($"MessageClient: {ex.Message}");
            }
        }

        public async Task Publish(ITeacherChannel channel, ICourseContent course)
        {
            if (channel is null) throw new ArgumentNullException(nameof(channel));
            if (course is null) throw new ArgumentNullException(nameof(course));

            SetupTeacherChannelIfMissing(channel.Teacher);

            var message = PrepareMessageToSend(course);
            _channel.BasicPublish(exchange: channel.Teacher,
                routingKey: channel.Domain ?? "",
                basicProperties: null,
                body: message);

            await Task.CompletedTask;
        }

        public void Subscribe(ITeacherChannel channel, MessagePublishedCallback callback)
        {
            _channel?.QueueBind(queue: GetQueueName(), exchange: channel.Teacher, routingKey: channel.Domain);

            _callback = callback;
            _consumer = new AsyncEventingBasicConsumer(_channel);
            _consumer.Received += OnMessageReceived;
            _channel.BasicConsume(queue: GetQueueName(), autoAck: true, consumer: _consumer);
        }
        public void Unsubscribe()
        {
            if (_consumer is not null)
                _consumer.Received -= OnMessageReceived;
        }

        private async Task OnMessageReceived(object? sender, BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var courseContent = ConvertReceivedMessage(body);

            var teacherChannel = new TeacherChannel
            {
                Teacher = ea.Exchange ?? "Unknown teacher",
                Domain = ea.RoutingKey ?? "Unknown domain",
            };
            await _callback?.Invoke(teacherChannel, courseContent);
        }

        private ICourseContent? ConvertReceivedMessage(byte[] message)
        {
            return JsonSerializer.Deserialize<CourseContent>(message);
        }

        private byte[] PrepareMessageToSend(ICourseContent course)
        {
            string message = JsonSerializer.Serialize(course);
            return Encoding.UTF8.GetBytes(message);
        }

        private readonly ConcurrentDictionary<string, bool> _teachers = new();
        private void SetupTeacherChannelIfMissing(string teacherName)
        {
            if (_teachers.ContainsKey(teacherName))
                return;

            _teachers[teacherName] = true;
            _channel?.ExchangeDeclare(exchange: teacherName, type: "topic");
        }

        private string? GetQueueName()
        {
            return _queueName ??= _channel?.QueueDeclare().QueueName;
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}