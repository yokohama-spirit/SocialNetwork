using ChannelsServiceLibrary.Application.Requests;
using ChannelsServiceLibrary.Domain.Interfaces;
using ChannelsServiceLibrary.Infrastructure.Data;
using ChannelsServiceLibrary.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
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
builder.Services.AddDbContext<ChannelConnect>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));





// Dependency injection
builder.Services.AddScoped<IChannelAdminsRepository, ChannelAdminsRepository>();
builder.Services.AddScoped<IChannelRepository, ChannelRepository>();
builder.Services.AddScoped<IUserSupport, UserSupportForChannels>();
builder.Services.AddScoped<IChannelSubsRepository, ChannelSubsRepository>();











//Mediator
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateChannelRequest).Assembly);
});





builder.Services.AddHttpContextAccessor(); 
builder.Services.AddHttpClient();








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
builder.Services.AddAutoMapper(typeof(ChannelsProfile));










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
