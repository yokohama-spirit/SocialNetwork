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
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AuthServiceLibrary.Application.Services
{
    public class LoginUserRequestHandle : IRequestHandler<LoginUserRequest, string>
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IJwtService _jwtService;
        private readonly ILogger<LoginUserRequestHandle> _logg;
        public LoginUserRequestHandle
            (SignInManager<User> signInManager,
            UserManager<User> userManager,
            IJwtService jwtService,
            ILogger<LoginUserRequestHandle> logg)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
            _logg = logg;
        }
        public async Task<string> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            _logg.LogInformation("Начинается идентификация пользователя...");
            var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, false);

            if (result.Succeeded)
            {
                _logg.LogInformation("Вход прошел успешно! Отдача JWT-токена...");
                var user = await _userManager
                    .FindByNameAsync(request.UserName) ?? throw new Exception("Пользователь не найден.");
                return await _jwtService.GenerateJwtTokenAsync(user);
            }
            else
            {
                throw new Exception("Данные указаны неверно.");
            }
        }
    }
}
