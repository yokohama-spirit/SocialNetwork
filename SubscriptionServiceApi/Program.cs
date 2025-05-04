using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PostServiceLibrary.Domain.Interfaces;
using RabbitMQ.Client;
using SubscriptionServiceLibrary.Application.Services;
using SubscriptionServiceLibrary.Domain.Interfaces;
using SubscriptionServiceLibrary.Infrastructure.Data;
using SubscriptionServiceLibrary.Infrastructure.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




//DataBase
builder.Services.AddDbContext<SubsConnect>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

 


// Dependency injection
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IUserSupport, UserSupportForSubs>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();









var rabbitMqConnection = new ConnectionFactory
{
    HostName = "localhost"
}.CreateConnection();

builder.Services.AddSingleton(rabbitMqConnection);










// Jwt
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Проверять издателя
            ValidateAudience = false, // Проверять получателя
            ValidateLifetime = true, // Проверять срок действия
            ValidateIssuerSigningKey = true, // Проверять подпись
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // Из appsettings.json
            ValidAudience = builder.Configuration["Jwt:Audience"], // Из appsettings.json
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!) // Секретный ключ
            )
        };
    });

builder.Services.AddAuthorization(); // Добавляем политики авторизации




builder.Services.AddHttpContextAccessor(); 
builder.Services.AddHttpClient();






var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
