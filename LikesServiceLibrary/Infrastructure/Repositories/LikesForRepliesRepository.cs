using LikesServiceLibrary.Domain.Entities;
using LikesServiceLibrary.Domain.Interfaces;
using LikesServiceLibrary.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PostServiceLibrary.Domain.Entities;
using PostServiceLibrary.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LikesServiceLibrary.Infrastructure.Repositories
{
    public class LikesForRepliesRepository : ILikesForRepliesRepository
    {
        private readonly LikesConnect _conn;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<LikesForPostsRepository> _logg;
        private readonly IUserSupport _support;
        public LikesForRepliesRepository
            (LikesConnect conn,
            IHttpClientFactory httpClientFactory,
            ILogger<LikesForPostsRepository> logg,
            IUserSupport support)
        {
            _conn = conn;
            _httpClientFactory = httpClientFactory;
            _logg = logg;
            _support = support;
        }

        public async Task AddLikeAsync(string replyId)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var serviceUrl = "http://localhost:5003";
            try
            {
                var response = await httpClient.GetFromJsonAsync<Post>(
                                    $"{serviceUrl}/api/replies/{replyId}",
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (response == null)
                {
                    throw new Exception("Ответ не найден.");
                }
                else
                {
                    var like = await _conn.Likes
                            .Where(l => l.ReplyId == replyId)
                            .FirstOrDefaultAsync();
                    if (like != null)
                    {
                        throw new Exception("Нельзя поставить лайк дважды.");
                    }
                    else
                    {
                        var newLike = new Like
                        {
                            ReplyId = replyId,
                            UserId = await _support.GetCurrentUserId()
                        };
                        await _conn.Likes.AddAsync(newLike);
                        _logg.LogInformation
                            ($"Лайк поставлен!\nИдентификатор ответа:{replyId}");
                        await _conn.SaveChangesAsync();
                    }
                }

            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ошибка HTTP запроса: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Like>> GetAllUserLikesForRepliesAsync(string userId)
        {
            return await _conn.Likes
                .Where(l => l.UserId == userId && l.ReplyId != null)
                .ToListAsync() ?? throw new Exception("Лайк не был поставлен.");
        }

        public async Task<Like> GetReplyLikeByIdAsync(string id)
        {
            return await _conn.Likes
                .Where(l => l.Id == id && l.ReplyId != null)
                .FirstOrDefaultAsync() ?? throw new Exception("Лайк не был поставлен.");
        }

        public async Task UnlikeAsync(string replyId)
        {
            _logg.LogInformation("Поиск лайка...");
            var like = await _conn.Likes
                .Where(l => l.ReplyId == replyId)
                .FirstOrDefaultAsync() ?? throw new Exception("Лайк не был поставлен.");
            var userId = await _support.GetCurrentUserId();
            if (like.UserId != userId)
            {
                throw new Exception("Нельзя убрать чужой лайк.");
            }
            else
            {
                _conn.Likes.Remove(like);
                _logg.LogInformation
                            ($"Лайк убран!\nИдентификатор ответа:{replyId}");
                await _conn.SaveChangesAsync();
            }
        }
    }
}
