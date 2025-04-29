using Microsoft.EntityFrameworkCore;
using SubscriptionServiceLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionServiceLibrary.Infrastructure.Data
{
    public class SubsConnect : DbContext
    {
        public SubsConnect(DbContextOptions<SubsConnect> options) : base(options) { }

        public DbSet<Subscription> Subscriptions { get; set; }
    }
}
