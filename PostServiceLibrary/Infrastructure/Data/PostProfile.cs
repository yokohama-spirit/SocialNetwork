using AuthServiceLibrary.Application.Requests;
using AuthServiceLibrary.Domain.Entities;
using PostServiceLibrary.Application.Requests;
using PostServiceLibrary.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostServiceLibrary.Infrastructure.Data
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<CreatePostRequest, Post>();
        }
    }
}
