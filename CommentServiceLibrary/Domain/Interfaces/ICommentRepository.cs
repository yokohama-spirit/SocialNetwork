using CommentServiceLibrary.Application.Requests.Comments;
using CommentServiceLibrary.Domain.Entities;
using PostServiceLibrary.Application.Requests;
using PostServiceLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommentServiceLibrary.Domain.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment> GetCommentByIdAsync(string id);
        Task<IEnumerable<Comment>> GetAllUserCommentsAsync(string userId);
        Task CreateCommentAsync(string postId, CreateCommentDTO request);
        Task UpdateCommentAsync(string postId, string commentId, UpdateCommentDTO request);
        Task DeleteCommentAsync(string postId, string commentId);
    }
}
