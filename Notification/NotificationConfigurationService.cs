using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Bson;
using Notification.INotifierServices;
using Notification.NotifierService;
using System;

namespace Notification
{
    public static class NotificationConfigurationServices
    {
        public static IServiceCollection AddNotification(this IServiceCollection services,Action<DbContextOptionsBuilder> optionsBuilder)
        {
            services.AddDbContextPool<NotificationDbContext.NotificationDbContext>(optionsBuilder);
            services.AddSignalR();
            services.AddScoped<IConnectionManager, ConnectionManager>();
            services.AddScoped<INotifier, Notifier>();

            using (var a = services.BuildServiceProvider().GetRequiredService<NotificationDbContext.NotificationDbContext>())
            {
                if (a.Database.EnsureCreated())
                {
                    a.Database.ExecuteSqlCommand($"truncate table NotificationConnections");
                }
            }
            return services;
        }


        public static void UseNotification(this IApplicationBuilder app)
        {
            app.UseSignalR(e => {
                e.MapHub<Notification>("/notification");
            });
        }
    }
}