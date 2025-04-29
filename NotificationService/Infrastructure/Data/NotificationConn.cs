using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Entities;
using System.Collections.Generic;

namespace NotificationService.Infrastructure.Data
{
    public class NotificationConn : DbContext
    {
        public NotificationConn(DbContextOptions<NotificationConn> options) : base(options) { }

        public DbSet<Notification> Notifications { get; set; }
    }
}
