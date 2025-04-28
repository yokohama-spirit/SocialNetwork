using CommentServiceLibrary.Application.Requests.Comments;
using CommentServiceLibrary.Application.Requests.Replies;
using CommentServiceLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommentServiceLibrary.Domain.Interfaces
{
    public interface IRepliesRepository
    {
        Task<Reply> GetReplyByIdAsync(string id);
        Task<IEnumerable<Reply>> GetAllUserRepliesAsync(string userId);
        Task CreateReplyAsync(string postId, string commentId, CreateReplyDTO request);
        Task UpdateReplyAsync
            (string replyId, UpdateReplyDTO request);
        Task DeleteReplyAsync(string replyId);
    }
}
