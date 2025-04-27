using AutoMapper;
using CommentServiceLibrary.Application.Requests;
using CommentServiceLibrary.Domain.Entities;
using PostServiceLibrary.Application.Requests;
using PostServiceLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommentServiceLibrary.Infrastructure.Data
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<CreateCommentDTO, Comment>();
        }
    }
}
