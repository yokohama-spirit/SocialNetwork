using SubscriptionServiceLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionServiceLibrary.Domain.Interfaces
{
    public interface ISubscriptionRepository
    {
        Task<Subscription> GetSubscriptionByIdAsync(string id);
        Task<IEnumerable<Subscription>> GetAllUserSubscriptionsAsync(string userId);
        Task SubscribeAsync(string followingId);
        Task UnsubscribeAsync(string followingId);
    }
}
