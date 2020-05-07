using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<FriendRequest>().HasIndex(e => new { e.RequestedFromId, e.RequestedToId }).IsUnique();
            base.OnModelCreating(builder);
        }

        public virtual DbSet<RelationKeys> RelationKeys { get; set; }
        public virtual DbSet<Message> Messages{ get; set; }
        public virtual DbSet<FriendRequest> FriendRequests { get; set; }
    }
}
