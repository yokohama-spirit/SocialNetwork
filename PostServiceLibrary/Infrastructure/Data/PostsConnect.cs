using AuthServiceLibrary.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PostServiceLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostServiceLibrary.Infrastructure.Data
{
    public class PostsConnect : DbContext
    {
        public PostsConnect(DbContextOptions<PostsConnect> options) : base(options) { }

        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>().HasKey(p => p.Id);
        }
    }
}
