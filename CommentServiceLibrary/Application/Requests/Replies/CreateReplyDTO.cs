using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommentServiceLibrary.Application.Requests.Replies
{
    public class CreateReplyDTO
    {
        public required string Content { get; set; }
    }
}
