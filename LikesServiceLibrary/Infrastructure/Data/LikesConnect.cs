using LikesServiceLibrary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikesServiceLibrary.Infrastructure.Data
{
    public class LikesConnect : DbContext
    {
        public LikesConnect(DbContextOptions<LikesConnect> options) : base(options) { }

        public DbSet<Like> Likes { get; set; }
    }
}
