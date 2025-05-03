using AuthServiceLibrary.Application.Requests;
using AuthServiceLibrary.Domain.Entities;
using AutoMapper;
using ChannelsServiceLibrary.Application.Requests;
using ChannelsServiceLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelsServiceLibrary.Infrastructure.Data
{
    public class ChannelsProfile : Profile
    {
        public ChannelsProfile()
        {
            CreateMap<CreateChannelRequest, Channel>();
            CreateMap<CreatePostDTO, ChannelPost>();
        }
    }
}
