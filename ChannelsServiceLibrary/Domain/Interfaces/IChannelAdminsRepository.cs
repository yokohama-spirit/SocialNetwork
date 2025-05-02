using ChannelsServiceLibrary.Application.Requests;
using ChannelsServiceLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelsServiceLibrary.Domain.Interfaces
{
    public interface IChannelAdminsRepository
    {
        Task<IEnumerable<ChannelAdmin>> GetAllChannelAdminsAsync(string channelId);
        Task AddChannelAdminAsync(string channelId, ChannelAdmin request);
        Task DeleteChannelAdminAsync(string channelId, string userId);
    }
}
