using CommentServiceLibrary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using PostServiceLibrary.Domain.Entities;
using PostServiceLibrary.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommentServiceLibrary.Infrastructure.Data
{
    public class CommentsConnect : DbContext
    {
        public CommentsConnect(DbContextOptions<CommentsConnect> options) : base(options) { }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reply> Replies { get; set; }
    }
}
