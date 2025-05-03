using AuthServiceLibrary.Domain.Entities;
using ChannelsServiceLibrary.Domain.Entities;
using ChannelsServiceLibrary.Domain.Interfaces;
using ChannelsServiceLibrary.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PostServiceLibrary.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChannelsServiceLibrary.Infrastructure.Repositories
{
    public class ChannelSubsRepository : IChannelSubsRepository
    {
        private readonly ChannelConnect _conn;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUserSupport _support;
        private readonly IChannelRepository _channelRep;

        public ChannelSubsRepository
             (ChannelConnect conn,
             IHttpClientFactory httpClientFactory,
             IUserSupport support,
             IChannelRepository channelRep)
        {
            _conn = conn;
            _httpClientFactory = httpClientFactory;
            _support = support;
            _channelRep = channelRep;
        }
        public async Task AddChannelSubAsync(string channelId, ChannelSubscriber command)
        {
            var channel = await _channelRep.GetChannelByIdAsync(channelId);

            var httpClient = _httpClientFactory.CreateClient();
            var serviceUrl = "http://localhost:5001";
            try
            {
                var response = await httpClient.GetFromJsonAsync<User>(
                                    $"{serviceUrl}/api/account/{command.UserId}",
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? throw new Exception("Пользователь не найден.");
                

                var userId = command.UserId;

                var existingSub = await _conn.Subscribers
                    .AnyAsync(s => s.UserId == userId && s.ChannelId == channelId);

                var isAdmin = await _conn.Admins
                    .AnyAsync(a => a.UserId == userId && a.ChannelId == channelId);

                if (existingSub || isAdmin || channel.MainAdminId == userId)
                {
                    throw new Exception
                      ("Нельзя подписать человека на канал если он уже подписан.");
                }


                var myId = await _support.GetCurrentUserId();
                var meAdmin = await _conn.Admins
                    .FirstOrDefaultAsync(a => a.UserId == myId && a.ChannelId == channelId);
                if (channel.MainAdminId == myId || meAdmin != null)
                {
                    command.ChannelId = channelId;
                    _conn.Subscribers.Add(command);
                    await _conn.SaveChangesAsync();
                }
                else
                {
                    throw new Exception
                    ("Нельзя добавить подписчика не являясь админом/гл.админом.");
                }


            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ошибка HTTP запроса: {ex.Message}");
            }
        }

        public async Task DeleteChannelSubAsync(string channelId, string subId)
        {
            var userId = await _support.GetCurrentUserId();
            var channel = await _channelRep.GetChannelByIdAsync(channelId);
            var admin = await _conn.Admins
                .FirstOrDefaultAsync(a => a.UserId == userId && a.ChannelId == channelId);
            var sub = await _conn.Subscribers
                .FirstOrDefaultAsync
                (a => a.UserId == subId && a.ChannelId == channelId);
            if(sub == null)
            {
                throw new Exception("Подписчик не найден.");
            }
            else
            {
                if (channel.MainAdminId == userId || admin != null)
                {
                    _conn.Subscribers.Remove(sub);
                    await _conn.SaveChangesAsync();
                }
                else
                {
                    throw new Exception
                    ("Нельзя удалить подписчика не являясь админом/гл.админом.");
                }
            }


        }

        public async Task<IEnumerable<ChannelSubscriber>> GetAllSubsAsync(string channelId)
        {
            return await _conn.Subscribers
                .Where(s => s.ChannelId == channelId)
                .ToListAsync();
        }

        public async Task<ChannelSubscriber?> GetChannelSubByIdAsync(string channelId, string subId)
        {
            return await _conn.Subscribers
                .FirstOrDefaultAsync(s => s.UserId == subId && s.ChannelId == channelId);
        }

        public async Task JoinChannelAsync(string channelId)
        {
            var channel = await _channelRep.GetChannelByIdAsync(channelId);
            var userId = await _support.GetCurrentUserId();

            var existingSub = await _conn.Subscribers
                .AnyAsync(s => s.UserId == userId && s.ChannelId == channelId);

            var isAdmin = await _conn.Admins
                .AnyAsync(a => a.UserId == userId && a.ChannelId == channelId);

            if (!existingSub && !isAdmin && channel.MainAdminId != userId)
            {
                var newSub = new ChannelSubscriber
                {
                    UserId = userId,
                    ChannelId = channelId
                };
                await _conn.Subscribers.AddAsync(newSub);
                await _conn.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Вы уже подписаны на канал или являетесь его администратором");
            }
        }


        public async Task LeaveChannelAsync(string channelId)
        {
            var userId = await _support.GetCurrentUserId();
            var channel = await _channelRep.GetChannelByIdAsync(channelId);
            var admin = await _conn.Admins
                .FirstOrDefaultAsync(a => a.UserId == userId && a.ChannelId == channelId);
            var sub = await _conn.Subscribers
                .FirstOrDefaultAsync
                (a => a.UserId == userId && a.ChannelId == channelId);
            if(sub == null && admin == null && channel.MainAdminId != userId)
            {
                throw new Exception("Нельзя покинуть канал не числясь в его подписчиках/админах.");
            }

            if(sub != null)
            {
                _conn.Subscribers.Remove(sub);
                await _conn.SaveChangesAsync();
            }
            else
            {
                if (admin != null)
                {
                    _conn.Admins.Remove(admin);
                    await _conn.SaveChangesAsync();
                }
                else
                {
                    if(channel.MainAdminId == userId)
                    {
                        throw new Exception("Нельзя покинуть канал являясь его создателем.");
                    }
                }
            }
        }
    }
}
