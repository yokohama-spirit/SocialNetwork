using Microsoft.AspNetCore.Http;
using PostServiceLibrary.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CommentServiceLibrary.Infrastructure.Repositories
{
    public class UserSupportForComments : IUserSupport
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserSupportForComments
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
