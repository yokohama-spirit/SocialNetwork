using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelsServiceLibrary.Domain.Entities
{
    public class ChannelAdmin
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string UserId { get; set; }
        public string? ChannelId { get; set; }
        public Channel? Channel { get; set; }
    }
}
