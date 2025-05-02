using ChannelsServiceLibrary.Application.Requests;
using ChannelsServiceLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelsServiceLibrary.Domain.Interfaces
{
    public interface IChannelRepository
    {
        Task<Channel> GetChannelByIdAsync(string channelId);
        Task<IEnumerable<Channel>> GetAllChannelsAsync();
        Task UpdateChannelNameAsync(string channelId, UpdateChannelNameDTO request);
        Task UpdateChannelDescAsync(string channelId, UpdateChannelDescriptionDTO request);
        Task DeleteChannelAsync(string channelId);
    }
}
