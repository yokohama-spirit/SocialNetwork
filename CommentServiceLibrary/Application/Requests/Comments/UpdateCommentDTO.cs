using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommentServiceLibrary.Application.Requests.Comments
{
    public class UpdateCommentDTO
    {
        public required string Content { get; set; }
    }
}
