using AuthServiceLibrary.Domain.Entities;
using CommentServiceLibrary.Application.Requests;
using CommentServiceLibrary.Domain.Entities;
using CommentServiceLibrary.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PostServiceLibrary.Domain.Interfaces;
using PostServiceLibrary.Infrastructure.Data;
using PostServiceLibrary.Infrastructure.Repositories;
using CommentServiceLibrary.Infrastructure.Data;
using AutoMapper;
using PostServiceLibrary.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CommentServiceLibrary.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CommentsConnect _conn;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CommentRepository> _logg;
        private readonly IMapper _mapper;
        private readonly IUserSupport _support;
        public CommentRepository
            (CommentsConnect conn,
            IHttpClientFactory httpClientFactory,
            ILogger<CommentRepository> logg,
            IUserSupport support,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _conn = conn;
            _httpClientFactory = httpClientFactory;
            _logg = logg;
            _support = support;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task CreateCommentAsync(string postId, CreateCommentDTO request)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var serviceUrl = "http://localhost:5002";
            try
            {
                var response = await httpClient.GetFromJsonAsync<Post>(
                                    $"{serviceUrl}/api/posts/{postId}",
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if(response == null)
                {
                    throw new Exception("Пост не найден.");
                }
                else
                {
                    _logg.LogInformation($"Creating comment: PostId={postId}, UserId={await _support.GetCurrentUserId()}, Content={request.Content}");
                    var comment = _mapper.Map<Comment>(request);
                    comment.PostId = postId;
                    comment.UserId = _httpContextAccessor.HttpContext?.User?
                    .FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

                    await _conn.Comments.AddAsync(comment);
                    await _conn.SaveChangesAsync();
                }
                
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ошибка HTTP запроса: {ex.Message}");
            }
        }

        public async Task DeleteCommentAsync(string postId, string commentId)
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
                    var comment = await _conn.Comments
                            .FindAsync(commentId) ?? throw new Exception("Комментарий не найден");
                    var userId = await _support.GetCurrentUserId();
                    if (comment.UserId != userId)
                    {
                        throw new Exception("Нельзя удалить чужой комментарий.");
                    }
                    else
                    {
                        _conn.Comments.Remove(comment);
                        await _conn.SaveChangesAsync();
                    }
                }

            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ошибка HTTP запроса: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Comment>> GetAllUserCommentsAsync(string userId)
        {
            return await _conn.Comments
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task<Comment> GetCommentByIdAsync(string id)
        {
            return await _conn.Comments
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync() ?? throw new Exception("Комментарий не найден.");
        }

        public async Task UpdateCommentAsync(string postId, string commentId, UpdateCommentDTO request)
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
                    var comment = await GetCommentByIdAsync(commentId);
                    var userId = await _support.GetCurrentUserId();
                    if (comment.UserId != userId)
                    {
                        throw new Exception("Нельзя удалить чужой комментарий.");
                    }
                    else
                    {
                        comment.Content = request.Content;
                        
                        await _conn.SaveChangesAsync();
                    }
                }

            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ошибка HTTP запроса: {ex.Message}");
            }
        }
    }
}
