using CommentServiceLibrary.Infrastructure.Data;
using LikesServiceLibrary.Domain.Interfaces;
using LikesServiceLibrary.Infrastructure.Data;
using LikesServiceLibrary.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PostServiceLibrary.Domain.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//DB
builder.Services.AddDbContext<LikesConnect>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));






builder.Services.AddScoped<IUserSupport, UserSupportForLikes>();
builder.Services.AddScoped<ILikesForPostsRepository, LikesForPostsRepository>();
builder.Services.AddScoped<ILikesForCommentsRepository, LikesForCommentsRepository>();
builder.Services.AddScoped<ILikesForRepliesRepository, LikesForRepliesRepository>();










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












builder.Services.AddHttpContextAccessor(); // Добавляет IHttpContextAccessor
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
