using ChannelsServiceLibrary.Application.Requests;
using ChannelsServiceLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelsServiceLibrary.Domain.Interfaces
{
    public interface IChannelSubsRepository
    {
        Task<ChannelSubscriber?> GetChannelSubByIdAsync(string channelId, string subId);
        Task<IEnumerable<ChannelSubscriber>> GetAllSubsAsync(string channelId);
        Task AddChannelSubAsync(string channelId, ChannelSubscriber command);
        Task JoinChannelAsync(string channelId);
        Task DeleteChannelSubAsync(string channelId, string subId);
    }
}
