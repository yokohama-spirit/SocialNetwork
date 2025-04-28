using Microsoft.AspNetCore.Http;
using PostServiceLibrary.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LikesServiceLibrary.Infrastructure.Repositories
{
    public class UserSupportForLikes : IUserSupport
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserSupportForLikes
            (IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User?
                    .FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }
    }
}
