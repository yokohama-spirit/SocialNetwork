using AuthServiceLibrary.Domain.Entities;
using ChannelsServiceLibrary.Domain.Entities;
using ChannelsServiceLibrary.Domain.Interfaces;
using ChannelsServiceLibrary.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PostServiceLibrary.Domain.Entities;
using PostServiceLibrary.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChannelsServiceLibrary.Infrastructure.Repositories
{
    public class ChannelAdminsRepository : IChannelAdminsRepository
    {
        private readonly ChannelConnect _conn;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUserSupport _support;
        private readonly IChannelRepository _channelRep;

        public ChannelAdminsRepository
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
        public async Task AddChannelAdminAsync(string channelId, ChannelAdmin request)
        {
            var channel = await _channelRep.GetChannelByIdAsync(channelId);

            var httpClient = _httpClientFactory.CreateClient();
            var serviceUrl = "http://localhost:5001";
            try
            {
                var response = await httpClient.GetFromJsonAsync<User>(
                                    $"{serviceUrl}/api/account/{request.UserId}",
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? throw new Exception("Пользователь не найден.");


                request.ChannelId = channelId;
                _conn.Admins.Add(request);
                await _conn.SaveChangesAsync();
                

            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ошибка HTTP запроса: {ex.Message}");
            }
        }

        public async Task DeleteChannelAdminAsync(string channelId, string userId)
        {
            var myId = await _support.GetCurrentUserId();
            var channel = await _channelRep.GetChannelByIdAsync(channelId);
            var admin = await _conn.Admins
                .Where(a => a.UserId == userId && a.ChannelId == channelId)
                .FirstOrDefaultAsync() ?? throw new Exception("Админ не найден.");
            if(channel.MainAdminId != myId)
            {
                throw new Exception("Нельзя удалить администратора не являясь создателем канала.");
            }
            else
            {
                _conn.Admins.Remove(admin);
                await _conn.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ChannelAdmin>> GetAllChannelAdminsAsync(string channelId)
        {
            return await _conn.Admins
                    .Where(a => a.ChannelId == channelId)
                    .ToListAsync() ?? throw new Exception("Админы не найдены.");
        }
    }
}
