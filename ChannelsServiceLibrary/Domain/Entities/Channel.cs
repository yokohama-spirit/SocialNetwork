using AuthServiceLibrary.Domain.Entities;
using ChannelsServiceLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelsServiceLibrary.Domain.Entities
{
    public class Channel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Name { get; set; }
        public string? Description { get; set; }
        public List<ChannelAdmin>? Admins { get; set; }
        public List<ChannelSubscriber>? Subscribers { get; set; }
        public List<ChannelPost>? Posts { get; set; }
        public ChannelType Type { get; set; } = ChannelType.Public;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Главный админ
        public string? MainAdminId { get; set; }
    }
}

