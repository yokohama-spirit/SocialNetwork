using LikesServiceLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikesServiceLibrary.Domain.Interfaces
{
    public interface ILikesForPostsRepository
    {
        Task<Like> GetPostLikeByIdAsync(string id);
        Task<IEnumerable<Like>> GetAllUserLikesForPostsAsync(string userId);
        Task AddLikeAsync(string postId);
        Task UnlikeAsync(string postId);
    }
}
