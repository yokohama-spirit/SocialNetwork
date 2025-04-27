using AuthServiceLibrary.Domain.Entities;
using AuthServiceLibrary.Infrastructure.Data;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PostServiceLibrary.Application.Requests;
using PostServiceLibrary.Domain.Entities;
using PostServiceLibrary.Domain.Interfaces;
using PostServiceLibrary.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PostServiceLibrary.Application.Services
{
    public class CreatePostRequestHandle : IRequestHandler<CreatePostRequest>
    {
        private readonly PostsConnect _conn;
        private readonly IMapper _mapper;
        private readonly ILogger<CreatePostRequestHandle> _logg;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserSupport _support;
        public CreatePostRequestHandle
            (PostsConnect conn,
            IMapper mapper,
            ILogger<CreatePostRequestHandle> logg,
            IHttpContextAccessor httpContextAccessor,
            IUserSupport support)
        {
            _conn = conn;
            _mapper = mapper;
            _logg = logg;
            _httpContextAccessor = httpContextAccessor;
            _support = support;
        }
        public async Task Handle(CreatePostRequest request, CancellationToken cancellationToken)
        {
            var userId = await _support.GetCurrentUserId();

            var post = _mapper.Map<Post>(request);
            post.UserId = userId.ToString();

            await _conn.Posts.AddAsync(post);
            await _conn.SaveChangesAsync();
        }
    }
}
