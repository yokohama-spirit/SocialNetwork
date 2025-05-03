using ChannelsServiceLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelsServiceLibrary.Domain.Interfaces
{
    public interface ICreatorService
    {
        Task<IEnumerable<JoinChannelRequest>> GetMyRequests(string channelId);
        Task SubmitRequest(string requestId);
        Task RejectRequest(string requestId);
    }
}
