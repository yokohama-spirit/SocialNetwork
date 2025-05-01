using Microsoft.EntityFrameworkCore;
using PostServiceLibrary.Domain.Interfaces;
using SubscriptionServiceLibrary.Domain.Interfaces;
using SubscriptionServiceLibrary.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionServiceLibrary.Application.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IUserSupport _support;
        private readonly SubsConnect _conn;
        public SubscriptionService(IUserSupport support, SubsConnect conn)
        {
            _support = support;
            _conn = conn;
        }
        public async Task<IEnumerable<string>> GetRecommendedUsers()
        {
            var userId = await _support.GetCurrentUserId();
            var followedUsers = await _conn.Subscriptions
                .Where(s => s.FollowerId == userId)
                .Select(s => s.FollowingId)
                .ToListAsync();

            var popularAuthors = await _conn.Subscriptions
                .GroupBy(s => s.FollowingId)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Where(id => id !=  userId && !followedUsers.Contains(id))
                .Take(10)
                .ToListAsync();
            var friendOfFriends = await _conn.Subscriptions
                .Where(s => followedUsers.Contains(s.FollowerId))
                .Select(s => s.FollowingId)
                .Take(10)
                .ToListAsync();
            return popularAuthors.Concat(friendOfFriends).ToList();
        }
    }
}
