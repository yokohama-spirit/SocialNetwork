using AuthServiceLibrary.Application.Requests;
using AuthServiceLibrary.Application.Services;
using AuthServiceLibrary.Domain.Entities;
using AuthServiceLibrary.Domain.Interfaces;
using AuthServiceLibrary.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Reflection;
using System.Text;
using UserServiceLibrary.Domain.Interfaces;
using UserServiceLibrary.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//DB
builder.Services.AddDbContext<DatabaseConnect>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpContextAccessor(); 




// Dependency injection
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();













//Mediator
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateUserRequest).Assembly);
});













// Identity settings
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    // Делаем Email обязательным и уникальным
    options.User.RequireUniqueEmail = true;

    // Настройки пароля (по умолчанию строгие)
    options.Password.RequireDigit = true;       // Требует цифру
    options.Password.RequireLowercase = true;   // Требует строчную букву
    options.Password.RequireUppercase = true;   // Требует заглавную букву
    options.Password.RequireNonAlphanumeric = true; // Требует спецсимвол (!@# и т. д.)
    options.Password.RequiredLength = 8;        // Минимум 8 символов
})
.AddEntityFrameworkStores<DatabaseConnect>()
.AddDefaultTokenProviders();







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











// Mapp settings
builder.Services.AddAutoMapper(typeof(UserProfile));





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