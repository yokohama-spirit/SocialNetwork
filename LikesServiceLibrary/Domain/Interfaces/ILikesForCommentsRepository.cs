using LikesServiceLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikesServiceLibrary.Domain.Interfaces
{
    public interface ILikesForCommentsRepository
    {
        Task<Like> GetCommentLikeByIdAsync(string id);
        Task<IEnumerable<Like>> GetAllUserLikesForCommentsAsync(string userId);
        Task AddLikeAsync(string commentId);
        Task UnlikeAsync(string commentId);
    }
}
