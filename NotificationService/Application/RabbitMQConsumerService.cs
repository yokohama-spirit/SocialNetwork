using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using NotificationService.Infrastructure.Data;
using System.Text.Json;
using System.Text;
using NotificationService.Domain.Entities;

namespace NotificationService.Application
{
    public class RabbitMQConsumerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RabbitMQConsumerService> _logger;

        public RabbitMQConsumerService(IServiceProvider serviceProvider, ILogger<RabbitMQConsumerService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare("notifications", false, false, false, null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<NotificationConn>();

                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var sub = JsonSerializer.Deserialize<Subscribe>(message);

                    await dbContext.Notifications.AddAsync(new Notification
                    {
                        Message = $"New subscribe! Follower ID: {sub.FollowerId}, following ID: {sub.FollowingId}!"
                    });
                    await dbContext.SaveChangesAsync();

                    _logger.LogInformation($"Processed: {message}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message");
                }
            };

            channel.BasicConsume("notifications", true, consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
