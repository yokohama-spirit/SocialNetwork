﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostServiceLibrary.Domain.Interfaces
{
    public interface IUserSupport
    {
        Task<string> GetCurrentUserId();
    }
}
