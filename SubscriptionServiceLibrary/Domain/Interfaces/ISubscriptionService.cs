using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionServiceLibrary.Domain.Interfaces
{
    public interface ISubscriptionService
    {
        Task<IEnumerable<string>> GetRecommendedUsers();
    }
}
