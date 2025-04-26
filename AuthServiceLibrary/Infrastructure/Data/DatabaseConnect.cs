using AuthServiceLibrary.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServiceLibrary.Infrastructure.Data
{
    public class DatabaseConnect : IdentityDbContext<User>
    {
        public DatabaseConnect(DbContextOptions<DatabaseConnect> options) : base(options) { }
    }
}
