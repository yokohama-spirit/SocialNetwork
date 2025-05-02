using ChannelsServiceLibrary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelsServiceLibrary.Infrastructure.Data
{
    public class ChannelConnect : DbContext
    {
        public ChannelConnect(DbContextOptions<ChannelConnect> options) : base(options) { }

        public DbSet<Channel> Channels { get; set; }
        public DbSet<ChannelPost> Posts { get; set; }
        public DbSet<ChannelComment> Comments { get; set; }

        //Admins
        public DbSet<ChannelAdmin> Admins { get; set; }

        //Subscribers
        public DbSet<ChannelSubscriber> Subscribers { get; set; }
    }
}
