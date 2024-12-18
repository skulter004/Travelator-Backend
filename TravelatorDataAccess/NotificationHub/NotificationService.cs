using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelatorDataAccess.Context;

namespace TravelatorDataAccess.NotificationHub
{
    public class Notification
    {
        public Guid UserId { get; set; }
        public string Message { get; set; }
    }
    public class NotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly TravelatorContext _context;

        public NotificationService(IHubContext<NotificationHub> hubContext, TravelatorContext context)
        {
            _hubContext = hubContext;
            _context = context;
        }

        public async Task HandleNotification(Notification notification)
        {
            string userId = notification.UserId.ToString();
            string? connectionId = NotificationHub.IsUserOnline(userId);
            if (connectionId!=null)
            {
                await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", notification.Message);
            }
            else
            {
                /*_context.Notifications.Add(new Notification
                {
                    UserId = notification.UserId,
                    Message = notification.Message,
                    Timestamp = notification.Timestamp
                });
                await _context.SaveChangesAsync();*/
                Console.WriteLine("User Offline");
            }
        }
    }
}
