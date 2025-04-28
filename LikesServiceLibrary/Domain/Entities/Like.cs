using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikesServiceLibrary.Domain.Entities
{
    public class Like
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? UserId { get; set; }
        public string? PostId { get; set; }
        public string? CommentId { get; set; }
        public string? ReplyId { get; set; }
    }
}
