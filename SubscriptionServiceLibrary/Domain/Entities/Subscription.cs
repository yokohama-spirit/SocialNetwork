using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionServiceLibrary.Domain.Entities
{
    public class Subscription
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string FollowerId { get; set; }
        public required string FollowingId { get; set; }
    }
}
