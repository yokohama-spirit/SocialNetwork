using ChannelsServiceLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelsServiceLibrary.Domain.Interfaces
{
    public interface IChannelCommentsRepository
    {
        Task<ChannelComment> GetChannelCommentByIdAsync
            (string postId, string commentId);
        Task<IEnumerable<ChannelComment>> GetAllCommentsAsync(string postId);
        Task AddCommentAsync(string channelId, string postId, ChannelComment command);
        Task DeleteCommentAsync(string channelId, string postId, string commentId);
        Task UpdateCommentAsync(string postId, string commentId, ChannelComment command);
    }
}
