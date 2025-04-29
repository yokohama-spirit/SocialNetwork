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
    public class LikesForCommentsRepository : ILikesForCommentsRepository
    {
        private readonly LikesConnect _conn;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<LikesForPostsRepository> _logg;
        private readonly IUserSupport _support;
        public LikesForCommentsRepository
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

        public async Task AddLikeAsync(string commentId)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var serviceUrl = "http://localhost:5003";
            try
            {
                var response = await httpClient.GetFromJsonAsync<Post>(
                                    $"{serviceUrl}/api/comments/{commentId}",
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (response == null)
                {
                    throw new Exception("Комментарий не найден.");
                }
                else
                {
                    var like = await _conn.Likes
                            .Where(l => l.CommentId == commentId)
                            .FirstOrDefaultAsync();
                    if (like != null)
                    {
                        throw new Exception("Нельзя поставить лайк дважды.");
                    }
                    else
                    {
                        var newLike = new Like
                        {
                            CommentId = commentId,
                            UserId = await _support.GetCurrentUserId()
                        };
                        await _conn.Likes.AddAsync(newLike);
                        _logg.LogInformation
                            ($"Лайк поставлен!\nИдентификатор комментария:{commentId}");
                        await _conn.SaveChangesAsync();
                    }
                }

            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ошибка HTTP запроса: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Like>> GetAllUserLikesForCommentsAsync(string userId)
        {
            return await _conn.Likes
                .Where(l => l.UserId == userId && l.CommentId != null)
                .ToListAsync() ?? throw new Exception("Лайк не был поставлен.");
        }

        public async Task<Like> GetCommentLikeByIdAsync(string id)
        {
            return await _conn.Likes
                .Where(l => l.Id == id && l.CommentId != null)
                .FirstOrDefaultAsync() ?? throw new Exception("Лайк не был поставлен.");
        }

        public async Task UnlikeAsync(string commentId)
        {
            _logg.LogInformation("Поиск лайка...");
            var like = await _conn.Likes
                .Where(l => l.CommentId == commentId)
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
                            ($"Лайк убран!\nИдентификатор комментария:{commentId}");
                await _conn.SaveChangesAsync();
            }
        }
    }
}
