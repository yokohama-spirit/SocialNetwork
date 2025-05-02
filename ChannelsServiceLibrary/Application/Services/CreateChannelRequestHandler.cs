using AutoMapper;
using ChannelsServiceLibrary.Application.Requests;
using ChannelsServiceLibrary.Domain.Entities;
using ChannelsServiceLibrary.Infrastructure.Data;
using MediatR;
using PostServiceLibrary.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelsServiceLibrary.Application.Services
{
    public class CreateChannelRequestHandler : IRequestHandler<CreateChannelRequest>
    {
        private readonly ChannelConnect _conn;
        private readonly IMapper _mapper;
        private readonly IUserSupport _support;
        public CreateChannelRequestHandler
            (ChannelConnect conn, 
            IMapper mapper, 
            IUserSupport support)
        {
            _conn = conn;
            _mapper = mapper;
            _support = support;
        }

        public async Task Handle(CreateChannelRequest request, CancellationToken cancellationToken)
        {
            var channel = _mapper.Map<Channel>(request);
            channel.MainAdminId = await _support.GetCurrentUserId();
            await _conn.Channels.AddAsync(channel);
            await _conn.SaveChangesAsync();
        }
    }
}
