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
    // ������ Email ������������ � ����������
    options.User.RequireUniqueEmail = true;

    // ��������� ������ (�� ��������� �������)
    options.Password.RequireDigit = true;       // ������� �����
    options.Password.RequireLowercase = true;   // ������� �������� �����
    options.Password.RequireUppercase = true;   // ������� ��������� �����
    options.Password.RequireNonAlphanumeric = true; // ������� ���������� (!@# � �. �.)
    options.Password.RequiredLength = 8;        // ������� 8 ��������
})
.AddEntityFrameworkStores<DatabaseConnect>()
.AddDefaultTokenProviders();







// Jwt
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // ��������� ��������
            ValidateAudience = false, // ��������� ����������
            ValidateLifetime = true, // ��������� ���� ��������
            ValidateIssuerSigningKey = true, // ��������� �������
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // �� appsettings.json
            ValidAudience = builder.Configuration["Jwt:Audience"], // �� appsettings.json
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!) // ��������� ����
            )
        };
    });

builder.Services.AddAuthorization(); // ��������� �������� �����������











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