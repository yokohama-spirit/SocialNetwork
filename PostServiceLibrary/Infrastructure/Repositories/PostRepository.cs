using AuthServiceLibrary.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PostServiceLibrary.Application.Requests;
using PostServiceLibrary.Application.Services;
using PostServiceLibrary.Domain.Entities;
using PostServiceLibrary.Domain.Interfaces;
using PostServiceLibrary.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PostServiceLibrary.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly PostsConnect _conn;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<PostRepository> _logg;
        private readonly IUserSupport _support;
        public PostRepository
            (PostsConnect conn,
            IHttpClientFactory httpClientFactory,
            ILogger<PostRepository> logg,
            IUserSupport support)
        {
            _conn = conn;
            _httpClientFactory = httpClientFactory;
            _logg = logg;
            _support = support;
        }
        public async Task DeletePostAsync(string id)
        {
            var post = await _conn.Posts
                .FindAsync(id) ?? throw new Exception("Пост не существует.");
            var userId = await _support.GetCurrentUserId();
            if(post.UserId != userId)
            {
                throw new Exception("Нельзя редактировать чужой пост.");
            }

            _conn.Posts.Remove(post);
            await _conn.SaveChangesAsync();

        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _conn.Posts
                .Include(p => p.Author)
                .ToListAsync() ?? throw new Exception("Постов пока нет.");
        }

        public async Task<IEnumerable<Post>> GetAllUserPostAsync(string id)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var serviceUrl = "http://localhost:5001";
            try
            {
                var response = await httpClient.GetFromJsonAsync<User>(
                                    $"{serviceUrl}/api/account/{id}",
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? throw new Exception("Сервер не дал ответа.");

                return await _conn.Posts
                    .Where(p => p.UserId == response.Id)
                    .Include(p => p.Author)
                    .ToListAsync();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ошибка HTTP запроса: {ex.Message}");
            }
        }

        public async Task<Post> GetPostByIdAsync(string id)
        {
            return await _conn.Posts
                    .Include(p => p.Author)
                    .FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("Пост не существует.");
        }

        public async Task UpdatePostAsync(string id, UpdatePostDTO updatedPost)
        {
            var post = await _conn.Posts
                    .FindAsync(id) ?? throw new Exception("Пост не существует.");

            var userId = await _support.GetCurrentUserId();
            if (post.UserId != userId)
            {
                throw new Exception("Нельзя редактировать чужой пост.");
            }

            post.Title = updatedPost.Title;
            post.Content = updatedPost.Content;
            await _conn.SaveChangesAsync();
        }
    }
}
