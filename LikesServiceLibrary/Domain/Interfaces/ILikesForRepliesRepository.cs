using LikesServiceLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikesServiceLibrary.Domain.Interfaces
{
    public interface ILikesForRepliesRepository
    {
        Task<Like> GetReplyLikeByIdAsync(string id);
        Task<IEnumerable<Like>> GetAllUserLikesForRepliesAsync(string userId);
        Task AddLikeAsync(string replyId);
        Task UnlikeAsync(string replyId);
    }
}
