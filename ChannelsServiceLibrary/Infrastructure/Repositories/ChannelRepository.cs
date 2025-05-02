using ChannelsServiceLibrary.Application.Requests;
using ChannelsServiceLibrary.Domain.Entities;
using ChannelsServiceLibrary.Domain.Interfaces;
using ChannelsServiceLibrary.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PostServiceLibrary.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Channel = ChannelsServiceLibrary.Domain.Entities.Channel;

namespace ChannelsServiceLibrary.Infrastructure.Repositories
{
    public class ChannelRepository : IChannelRepository
    {
        private readonly IUserSupport _support;
        private readonly ChannelConnect _conn;
        public ChannelRepository
            (ChannelConnect conn, 
            IUserSupport support)
        {
            _conn = conn;
            _support = support;
        }

        public async Task DeleteChannelAsync(string channelId)
        {
            var userId = await _support.GetCurrentUserId();
            var channel = await GetChannelByIdAsync(channelId);
            if (channel.MainAdminId != userId)
            {
                throw new Exception("Нельзя удалить чужой канал.");
            }
            else
            {
                _conn.Channels.Remove(channel);
                await _conn.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Channel>> GetAllChannelsAsync()
        {
            return await _conn.Channels
                .ToListAsync();
        }

        public async Task<Channel> GetChannelByIdAsync(string channelId)
        {
            return await _conn.Channels
                .FindAsync(channelId) ?? throw new Exception("Канала не существует.");
        }

        public async Task UpdateChannelDescAsync(string channelId, UpdateChannelDescriptionDTO request)
        {
            var userId = await _support.GetCurrentUserId();
            var channel = await GetChannelByIdAsync(channelId);
            var admin = await _conn.Admins
                .FirstOrDefaultAsync(a => a.UserId ==  userId && a.ChannelId == channelId);

            if (channel.MainAdminId == userId || admin != null)
            {
                channel.Description = request.Description;
                await _conn.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Нельзя изменить чужой канал.");
            }
        }

        public async Task UpdateChannelNameAsync(string channelId, UpdateChannelNameDTO request)
        {
            var userId = await _support.GetCurrentUserId();
            var channel = await GetChannelByIdAsync(channelId);
            var admin = await _conn.Admins
                .FirstOrDefaultAsync(a => a.UserId == userId && a.ChannelId == channelId);

            if (channel.MainAdminId == userId || admin != null)
            {
                channel.Name = request.Name;
                await _conn.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Нельзя изменить чужой канал.");
            }
        }
    }
}
