using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelsServiceLibrary.Domain.Entities
{
    public class ChannelPost
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Content { get; set; }
        public string? ChannelId { get; set; }
        public Channel? Channel { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<ChannelComment>? Comments { get; set; }
    }
}
