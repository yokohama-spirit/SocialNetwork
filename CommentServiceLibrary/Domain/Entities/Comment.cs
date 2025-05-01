using AuthServiceLibrary.Domain.Entities;
using PostServiceLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommentServiceLibrary.Domain.Entities
{
    public class Comment
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Content { get; set; }
        public string? PostId { get; set; }
        public string? UserId { get; set; }
        public DateTime CreatedAd { get; set; } = DateTime.UtcNow;
        public List<Reply>? Replies { get; set; }
    }
}
