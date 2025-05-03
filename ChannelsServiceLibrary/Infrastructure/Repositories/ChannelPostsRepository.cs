using AuthServiceLibrary.Domain.Entities;
using AutoMapper;
using ChannelsServiceLibrary.Application.Requests;
using ChannelsServiceLibrary.Domain.Entities;
using ChannelsServiceLibrary.Domain.Interfaces;
using ChannelsServiceLibrary.Infrastructure.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PostServiceLibrary.Domain.Entities;
using PostServiceLibrary.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ChannelsServiceLibrary.Infrastructure.Repositories
{
    public class ChannelPostsRepository : IChannelPostsRepository
    {
        private readonly ChannelConnect _conn;
        private readonly IUserSupport _support;
        private readonly IChannelRepository _channelRep;
        private readonly IMapper _mapper;

        public ChannelPostsRepository
             (ChannelConnect conn,
             IUserSupport support,
             IChannelRepository channelRep,
             IMapper mapper)
        {
            _conn = conn;
            _support = support;
            _channelRep = channelRep;
            _mapper = mapper;
        }
        public async Task AddPostAsync(string channelId, CreatePostDTO command)
        {
            var userId = await _support.GetCurrentUserId();
            var channel = await _channelRep.GetChannelByIdAsync(channelId);
            var admin = await _conn.Admins
                .FirstOrDefaultAsync(a => a.UserId == userId && a.ChannelId == channelId);
            if (channel.MainAdminId == userId || admin != null)
            {
                var post = _mapper.Map<ChannelPost>(command);
                post.UserId = userId;
                post.ChannelId = channelId;
                await _conn.Posts.AddAsync(post);
                await _conn.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Нельзя опубликовать пост не являясь админом/создателем канала.");
            }
        }

        public async Task DeletePostAsync(string channelId, string postId)
        {
            var post = await GetChannelPostByIdAsync(channelId, postId);

            var userId = await _support.GetCurrentUserId();
            var channel = await _channelRep.GetChannelByIdAsync(channelId);
            var admin = await _conn.Admins
                .FirstOrDefaultAsync(a => a.UserId == userId && a.ChannelId == channelId);
            if (channel.MainAdminId == userId || admin != null)
            {
                _conn.Posts.Remove(post);
                await _conn.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Нельзя удалить пост не являясь админом/создателем канала.");
            }
        }

        public async Task<IEnumerable<ChannelPost>> GetAllPostsAsync(string channelId)
        {
            var channel = await _channelRep.GetChannelByIdAsync(channelId);
            if(channel.Type == ChannelType.Public)
            {
                return await _conn.Posts
                .Where(p => p.ChannelId == channelId)
                .ToListAsync();
            }
            else
            {
                var userId = await _support.GetCurrentUserId();
                var admin = await _conn.Admins
                    .FirstOrDefaultAsync(a => a.UserId == userId && a.ChannelId == channelId);
                var sub = await _conn.Subscribers
                        .FirstOrDefaultAsync
                        (a => a.UserId == userId && a.ChannelId == channelId);
                if(admin != null || sub != null || channel.MainAdminId == userId)
                {
                    return await _conn.Posts
                    .Where(p => p.ChannelId == channelId)
                    .ToListAsync();
                }
                else
                {
                    throw new Exception
                        ("Нельзя просмотреть посты закрытого канала не являясь его участником.");
                }
            }

        }

        public async Task<ChannelPost> GetChannelPostByIdAsync(string channelId, string postId)
        {
            return await _conn.Posts
                    .FirstOrDefaultAsync
                    (p => p.Id == postId && p.ChannelId == channelId)
                    ?? throw new Exception("Поста не существует");
        }

        public async Task UpdatePostAsync(string channelId, string postId, ChannelPost command)
        {
            var post = await GetChannelPostByIdAsync(channelId, postId);

            var userId = await _support.GetCurrentUserId();
            var channel = await _channelRep.GetChannelByIdAsync(channelId);
            var admin = await _conn.Admins
                .FirstOrDefaultAsync(a => a.UserId == userId && a.ChannelId == channelId);
            if (channel.MainAdminId == userId || admin != null)
            {
                post.Content = command.Content;
                await _conn.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Нельзя изменить пост не являясь админом/создателем канала.");
            }
        }
    }
}
