using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommentServiceLibrary.Domain.Entities
{
    public class Reply
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Content { get; set; }
        public string? UserId { get; set; }
        public string? CommentId { get; set; }
        public DateTime CreatedAd { get; set; } = DateTime.Now;
        public Comment? Comment { get; set; }
    }
}
