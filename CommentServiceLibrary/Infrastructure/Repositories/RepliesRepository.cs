using AutoMapper;
using CommentServiceLibrary.Application.Requests.Replies;
using CommentServiceLibrary.Domain.Entities;
using CommentServiceLibrary.Domain.Interfaces;
using CommentServiceLibrary.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PostServiceLibrary.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommentServiceLibrary.Infrastructure.Repositories
{
    public class RepliesRepository : IRepliesRepository
    {
        private readonly CommentsConnect _conn;
        private readonly ILogger<CommentRepository> _logg;
        private readonly IMapper _mapper;
        private readonly IUserSupport _support;
        public RepliesRepository
            (CommentsConnect conn,
            IHttpClientFactory httpClientFactory,
            ILogger<CommentRepository> logg,
            IUserSupport support,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _conn = conn;
            _logg = logg;
            _support = support;
            _mapper = mapper;
        }
        public async Task CreateReplyAsync(string postId, string commentId, CreateReplyDTO request)
        {
            _logg.LogInformation("Начинается поиск комментария...");

            var comment = _conn.Comments
                .Where(c => c.Id == commentId && c.PostId == postId)
                .FirstOrDefaultAsync() ?? throw new Exception("Комментарий не существует.");
            var reply = _mapper.Map<Reply>(request);
            reply.CommentId = commentId;
            reply.UserId = await _support.GetCurrentUserId();

            _logg.LogInformation
                ($"Ответ успешно создан!\nЕго содержание: {reply.Content}");

            await _conn.Replies.AddAsync(reply);
            await _conn.SaveChangesAsync();
        }

        public async Task DeleteReplyAsync(string replyId)
        {
            _logg.LogInformation("Начинается поиск ответа...");
            var reply = await GetReplyByIdAsync(replyId);
            var userId = await _support.GetCurrentUserId();
            if (reply.UserId != userId)
            {
                throw new Exception("Нельзя изменить чужой ответ на комментарий.");
            }
            else
            {
                _conn.Replies.Remove(reply);
                _logg.LogInformation
                    ($"Ответ успешно удален! Идентификатор ответа: {replyId}");
                await _conn.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Reply>> GetAllUserRepliesAsync(string userId)
        {
            return await _conn.Replies
                    .Where(c => c.UserId == userId)
                    .ToListAsync();
        }

        public async Task<Reply> GetReplyByIdAsync(string id)
        {
            return await _conn.Replies
                .FindAsync(id) ?? throw new NotImplementedException();
        }

        public async Task UpdateReplyAsync(string replyId, UpdateReplyDTO request)
        {
            _logg.LogInformation("Начинается поиск ответа...");
            var reply = await GetReplyByIdAsync(replyId);
            var userId = await _support.GetCurrentUserId();
            if (reply.UserId != userId)
            {
                throw new Exception("Нельзя изменить чужой ответ на комментарий.");
            }
            else
            {
                reply.Content = request.Content;
                _logg.LogInformation
                    ($"Ответ успешно изменен!\nЕго новое содержание: {request.Content}");
                await _conn.SaveChangesAsync();
            }
        }
    }
}
