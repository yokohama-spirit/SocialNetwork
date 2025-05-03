using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelsServiceLibrary.Domain.Entities
{
    public class JoinChannelRequest
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string SubscriberId { get; set; }
        public required string MainAdminId { get; set; }
        public required string ChannelId { get; set; }
    }
}
