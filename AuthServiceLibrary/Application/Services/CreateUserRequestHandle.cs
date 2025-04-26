using AuthServiceLibrary.Application.Requests;
using AuthServiceLibrary.Domain.Entities;
using AuthServiceLibrary.Domain.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServiceLibrary.Application.Services
{
    public class CreateUserRequestHandle : IRequestHandler<CreateUserRequest, string>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IJwtService _jwtService;
        private readonly ILogger<CreateUserRequestHandle> _logg;
        public CreateUserRequestHandle
            (IMapper mapper,
            UserManager<User> userManager,
            IJwtService jwtService,
            ILogger<CreateUserRequestHandle> logg)
        {
            _mapper = mapper;
            _userManager = userManager;
            _jwtService = jwtService;
            _logg = logg;
        }
        public async Task<string> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.FindByNameAsync(request.UserName);
            if (existingUser != null)
            {
                _logg.LogError($"Пользователь {request.UserName} уже существует");
                throw new Exception("Пользователь уже существует");
            }

            var user = _mapper.Map<User>(request);

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logg.LogError($"Ошибка создания пользователя: {errors}");
                throw new Exception($"Ошибка создания: {errors}");
            }

            _logg.LogInformation($"Пользователь {user.UserName} успешно создан (ID: {user.Id})");

            var token = await _jwtService.GenerateJwtTokenAsync(user);
            _logg.LogInformation("Пользователь успешно создан! Отдача JWT-токена...");
            return token;
        }
    }
}
