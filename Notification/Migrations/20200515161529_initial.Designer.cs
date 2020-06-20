﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Notification.NotificationDbContext;

namespace Notification.Migrations
{
    [DbContext(typeof(NotificationDbContext.NotificationDbContext))]
    [Migration("20200515161529_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("Notification.Model.NotificationConnection", b =>
                {
                    b.Property<string>("ConnectionId");

                    b.Property<string>("UserId");

                    b.HasKey("ConnectionId", "UserId");

                    b.ToTable("NotificationConnections");
                });
#pragma warning restore 612, 618
        }
    }
}