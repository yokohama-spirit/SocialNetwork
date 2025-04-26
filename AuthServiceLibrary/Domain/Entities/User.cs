using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServiceLibrary.Domain.Entities
{
    public class User : IdentityUser
    {
        public required override string Email { get; set; }
    }
}
