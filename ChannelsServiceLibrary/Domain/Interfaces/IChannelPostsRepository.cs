using ChannelsServiceLibrary.Application.Requests;
using ChannelsServiceLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelsServiceLibrary.Domain.Interfaces
{
    public interface IChannelPostsRepository
    {
        Task<ChannelPost> GetChannelPostByIdAsync(string channelId, string postId);
        Task<IEnumerable<ChannelPost>> GetAllPostsAsync(string channelId);
        Task AddPostAsync(string channelId, CreatePostDTO command);
        Task DeletePostAsync(string channelId, string postId);
        Task UpdatePostAsync(string channelId, string postId, ChannelPost command);
    }
}
