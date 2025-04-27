using Microsoft.AspNetCore.Http;
using PostServiceLibrary.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PostServiceLibrary.Infrastructure.Repositories
{
    public class UserSupport : IUserSupport
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserSupport
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
