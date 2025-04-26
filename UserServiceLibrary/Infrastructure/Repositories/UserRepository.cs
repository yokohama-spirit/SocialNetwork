
using AuthServiceLibrary.Domain.Entities;
using AuthServiceLibrary.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserServiceLibrary.Domain.Interfaces;

namespace UserServiceLibrary.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DatabaseConnect _conn;
        private readonly UserManager<User> _userManager;
        public UserRepository
            (IHttpContextAccessor httpContextAccessor,
            DatabaseConnect conn,
            UserManager<User> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _conn = conn;
            _userManager = userManager;
        }
        public async Task<User> GetCurrentUser()
        {
            var userId = await GetCurrentUserId();
            return await _userManager
                .FindByIdAsync(userId) ?? throw new InvalidOperationException("Текущий пользователь не найден.");
        }

        public async Task<string> GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User?
                   .FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id) ?? throw new InvalidOperationException("Пользователь не найден.");
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _userManager.Users
                    .ToListAsync() ?? throw new InvalidOperationException("Пользователи не найден.");

        }
    }
}
