using AuthServiceLibrary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PostServiceLibrary.Domain.Entities;
using PostServiceLibrary.Domain.Interfaces;
using SubscriptionServiceLibrary.Domain.Entities;
using SubscriptionServiceLibrary.Domain.Interfaces;
using SubscriptionServiceLibrary.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SubscriptionServiceLibrary.Infrastructure.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly SubsConnect _conn;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<SubscriptionRepository> _logg;
        private readonly IUserSupport _support;
        public SubscriptionRepository
            (SubsConnect conn,
            IHttpClientFactory httpClientFactory,
            ILogger<SubscriptionRepository> logg,
            IUserSupport support)
        {
            _conn = conn;
            _httpClientFactory = httpClientFactory;
            _logg = logg;
            _support = support;
        }

        public async Task<IEnumerable<Subscription>> GetAllUserSubscriptionsAsync(string userId)
        {
            return await _conn.Subscriptions
                    .Where(s => s.FollowerId == userId)
                    .ToListAsync();
        }

        public async Task<Subscription> GetSubscriptionByIdAsync(string id)
        {
            return await _conn.Subscriptions
                    .FindAsync(id) ?? throw new Exception("Подписка не найдена");
        }

        public async Task SubscribeAsync(string followingId)
        {
            var userId = await _support.GetCurrentUserId();
            var httpClient = _httpClientFactory.CreateClient();
            var serviceUrl = "http://localhost:5001";
            try
            {
                var response = await httpClient.GetFromJsonAsync<User>(
                                    $"{serviceUrl}/api/account/{followingId}",
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (response == null || response.Id == userId)
                {
                    throw new Exception("Пользователь не найден/некорректен");
                }
                else
                {
                    var sub = await _conn.Subscriptions
                            .Where(s => s.FollowingId == followingId && s.FollowerId == userId)
                            .FirstOrDefaultAsync();
                    if (sub != null)
                    {
                        throw new Exception("Нельзя подписаться дважды.");
                    }
                    else
                    {
                        var newSub = new Subscription
                        {
                            FollowerId = userId,
                            FollowingId = followingId
                        };
                        await _conn.Subscriptions.AddAsync(newSub);
                        await _conn.SaveChangesAsync();
                    }
                }

            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ошибка HTTP запроса: {ex.Message}");
            }
        }

        public async Task UnsubscribeAsync(string followingId)
        {
            _logg.LogInformation("Поиск подписки...");
            var userId = await _support.GetCurrentUserId();
            var sub = await _conn.Subscriptions
                    .Where(s => s.FollowingId == followingId && s.FollowerId == userId)
                    .FirstOrDefaultAsync() ?? throw new Exception("Подписка не найдена");
            if (sub.FollowerId != userId)
            {
                throw new Exception("Невозможно удалить чужую подписку.");
            }
            else
            {
                _conn.Subscriptions.Remove(sub);
                _logg.LogInformation
                            ($"Подписка удалена!\nИдентификатор подписки:{followingId}");
                await _conn.SaveChangesAsync();
            }
        }
    }
}
