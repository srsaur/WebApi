using Microsoft.EntityFrameworkCore;
using Notification.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.NotificationDbContext
{
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NotificationConnection>(e => e.HasKey(z => new { z.ConnectionId, z.UserId }));
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<NotificationConnection> NotificationConnections{get;set;}
    }
}
