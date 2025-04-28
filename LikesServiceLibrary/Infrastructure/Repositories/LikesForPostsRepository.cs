using AutoMapper;
using CommentServiceLibrary.Domain.Entities;
using LikesServiceLibrary.Domain.Entities;
using LikesServiceLibrary.Domain.Interfaces;
using LikesServiceLibrary.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Http;
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
    public class LikesForPostsRepository : ILikesForPostsRepository
    {
        private readonly LikesConnect _conn;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<LikesForPostsRepository> _logg;
        private readonly IUserSupport _support;
        public LikesForPostsRepository
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

        public async Task AddLikeAsync(string postId)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var serviceUrl = "http://localhost:5002";
            try
            {
                var response = await httpClient.GetFromJsonAsync<Post>(
                                    $"{serviceUrl}/api/posts/{postId}",
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (response == null)
                {
                    throw new Exception("Пост не найден.");
                }
                else
                {
                    var like = await _conn.Likes
                            .Where(l => l.PostId == postId)
                            .FirstOrDefaultAsync();
                    if (like != null)
                    {
                        throw new Exception("Нельзя поставить лайк дважды.");
                    }
                    else
                    {
                        var newLike = new Like
                        {
                            PostId = postId,
                            UserId = await _support.GetCurrentUserId()
                        };
                        await _conn.Likes.AddAsync(newLike);
                        _logg.LogInformation
                            ($"Лайк поставлен!\nИдентификатор поста:{postId}");
                        await _conn.SaveChangesAsync();
                    }
                }

            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ошибка HTTP запроса: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Like>> GetAllUserLikesForPostsAsync(string userId)
        {
            return await _conn.Likes
                .Where(l => l.UserId == userId && l.PostId != null) 
                .ToListAsync() ?? throw new Exception("Лайк не был поставлен.");
        }

        public async Task<Like> GetPostLikeByIdAsync(string id)
        {
            return await _conn.Likes
                .Where(l => l.Id == id && l.PostId != null)
                .FirstOrDefaultAsync() ?? throw new Exception("Лайк не был поставлен.");
        }

        public async Task UnlikeAsync(string postId)
        {
            _logg.LogInformation("Поиск лайка...");
            var like = await _conn.Likes
                .Where(l => l.PostId == postId)
                .FirstOrDefaultAsync() ?? throw new Exception("Лайк не был поставлен.");
            var userId = await _support.GetCurrentUserId();
            if(like.UserId != userId)
            {
                throw new Exception("Нельзя убрать чужой лайк.");
            }
            else
            {
                _conn.Likes.Remove(like);
                _logg.LogInformation
                            ($"Лайк убран!\nИдентификатор поста:{postId}");
                await _conn.SaveChangesAsync();
            }
        }
    }
}
