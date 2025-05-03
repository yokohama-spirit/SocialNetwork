using ChannelsServiceLibrary.Domain.Entities;
using ChannelsServiceLibrary.Domain.Interfaces;
using ChannelsServiceLibrary.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using PostServiceLibrary.Domain.Entities;
using PostServiceLibrary.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ChannelsServiceLibrary.Infrastructure.Repositories
{
    public class ChannelCommentsRepository : IChannelCommentsRepository
    {
        private readonly ChannelConnect _conn;
        private readonly IUserSupport _support;
        private readonly IChannelPostsRepository _postRep;
        private readonly IChannelRepository _channelRep;

        public ChannelCommentsRepository
             (ChannelConnect conn,
             IUserSupport support,
             IChannelPostsRepository postRep,
             IChannelRepository channelRep)
        {
            _conn = conn;
            _support = support;
            _postRep = postRep;
            _channelRep = channelRep;
        }
        public async Task AddCommentAsync(string channelId, string postId, ChannelComment command)
        {
            var post = await _postRep.GetChannelPostByIdAsync(channelId, postId);
            var userId = await _support.GetCurrentUserId();
            command.UserId = userId;
            command.PostId = postId;
            await _conn.Comments.AddAsync(command);
            await _conn.SaveChangesAsync();
        }
        
        public async Task DeleteCommentAsync(string channelId, string postId, string commentId)
        {
            var channel = await _channelRep.GetChannelByIdAsync(channelId);

            var userId = await _support.GetCurrentUserId();
            var admin = await _conn.Admins
                .FirstOrDefaultAsync(a => a.UserId == userId && a.ChannelId == channelId);
            var comment = await GetChannelCommentByIdAsync(postId, commentId);

            if (comment.UserId == userId || admin != null || channel.MainAdminId == userId)
            {
                _conn.Comments.Remove(comment);
                await _conn.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Нельзя удалить чужой комментарий.");
            }
        }

        public async Task<IEnumerable<ChannelComment>> GetAllCommentsAsync(string postId)
        {
            return await _conn.Comments
                .Where(c => c.PostId == postId)
                .ToListAsync();
        }

        public async Task<ChannelComment> GetChannelCommentByIdAsync(string postId, string commentId)
        {
            return await _conn.Comments
                .FirstOrDefaultAsync
                (c => c.PostId == postId && c.Id == commentId)
                ?? throw new Exception("Комментария не существует");
        }

        public async Task UpdateCommentAsync(string postId, string commentId, ChannelComment command)
        {
            var comment = await GetChannelCommentByIdAsync(postId, commentId);
            var userId = await _support.GetCurrentUserId();

            if (comment.UserId == userId)
            {
                comment.Content = command.Content;
                await _conn.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Нельзя изменить чужой комментарий.");
            }
        }
    }
}
