﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostServiceLibrary.Application.Requests
{
    public class UpdatePostDTO
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
    }
}
