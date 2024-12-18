using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using TravelatorDataAccess.Interfaces;

namespace TravelatorDataAccess.NotificationHub
{
    public class RabbitMQService: BackgroundService, INotificationPublisher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConnectionFactory _factory;
        private IConnection? _connection;
        private IChannel? _channel;

        public RabbitMQService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            await NotificationListener("cab_bookings");
            await NotificationListener("travel_request");
        }
        public async void PublishNotification(Guid userId, string message, string queue)
        {
            _connection = await _factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            await _channel.QueueDeclareAsync(queue: queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
            Notification notification = new Notification
            {
                UserId = userId,
                Message = message
            };

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(notification));

            await _channel.BasicPublishAsync(exchange: "", routingKey: queue, body: body);
        }

        public async Task NotificationListener(string queue)
        {
            _connection = await _factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            await _channel.QueueDeclareAsync(queue: queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var jsonString = Encoding.UTF8.GetString(body);
                var notification = JsonConvert.DeserializeObject<Notification>(jsonString);
                using var scope = _serviceProvider.CreateScope();
                var notificationService = scope.ServiceProvider.GetRequiredService<NotificationService>();
                notificationService.HandleNotification(notification);
                return Task.CompletedTask;
            };
            await _channel.BasicConsumeAsync(queue: queue, autoAck: true, consumer: consumer);
        }
        public override void Dispose()
        {
            _channel?.CloseAsync();
            _connection?.CloseAsync();
            base.Dispose();
        }
    }
}
