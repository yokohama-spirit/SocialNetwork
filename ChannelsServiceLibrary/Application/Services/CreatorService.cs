using AuthServiceLibrary.Domain.Entities;
using ChannelsServiceLibrary.Domain.Entities;
using ChannelsServiceLibrary.Domain.Interfaces;
using ChannelsServiceLibrary.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using PostServiceLibrary.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ChannelsServiceLibrary.Application.Services
{
    public class CreatorService : ICreatorService
    {
        private readonly ChannelConnect _conn;
        private readonly IUserSupport _support;
        public CreatorService(ChannelConnect conn, IUserSupport support)
        {
            _conn = conn;
            _support = support;
        }
        public async Task<IEnumerable<JoinChannelRequest>> GetMyRequests(string channelId)
        {
            var userId = await _support.GetCurrentUserId();
            return await _conn.JoinRequests
                .Where(r => r.MainAdminId == userId && r.ChannelId == channelId)
                .ToListAsync();
        }

        public async Task RejectRequest(string requestId)
        {
            var userId = await _support.GetCurrentUserId();
            var request = await _conn.JoinRequests
                .FindAsync(requestId) ?? throw new Exception("Заявка не найдена.");
            if(request.MainAdminId == userId)
            {
                _conn.JoinRequests.Remove(request);
                await _conn.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Нельзя принять/отклонить заявку не являясь создателем канала.");
            }
        }

        public async Task SubmitRequest(string requestId)
        {
            var userId = await _support.GetCurrentUserId();
            var request = await _conn.JoinRequests
                .FindAsync(requestId) ?? throw new Exception("Заявка не найдена.");
            if (request.MainAdminId == userId)
            {
                var newSub = new ChannelSubscriber
                {
                    UserId = userId,
                    ChannelId = request.ChannelId
                };
                await _conn.Subscribers.AddAsync(newSub);
                await _conn.SaveChangesAsync();

                _conn.JoinRequests.Remove(request);
                await _conn.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Нельзя принять/отклонить заявку не являясь создателем канала.");
            }
        }
    }
}
