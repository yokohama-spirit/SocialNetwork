using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using NotificationService.Application;
using NotificationService.Controllers;
using NotificationService.Domain.Entities;
using NotificationService.Infrastructure.Data;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Запускаем фоновый сервис для прослушивания RabbitMQ
var factory = new ConnectionFactory { HostName = "localhost" };
var connection = factory.CreateConnection();
var channel = connection.CreateModel();

// Объявляем очередь перед использованием
channel.QueueDeclare(
    queue: "notifications",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);

builder.Services.AddDbContext<NotificationConn>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHostedService<RabbitMQConsumerService>();

var app = builder.Build();


/*// Получаем NotificationConn из сервисов
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<NotificationConn>();

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var sub = JsonSerializer.Deserialize<Subscribe>(message);

        // Используем dbContext вместо NotificationController._conn
        dbContext.Notifications.Add(new Notification
        {
            Message = $"New subscribe! Follower ID: {sub.FollowerId}, following ID: {sub.FollowingId}!"
        });
        dbContext.SaveChanges(); // Не забудьте сохранить!

        Console.WriteLine($" [x] Received {message}");
    };

    channel.BasicConsume(queue: "notifications", autoAck: true, consumer: consumer);
}*/

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
