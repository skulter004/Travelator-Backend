using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
namespace TravelatorDataAccess.NotificationHub
{
    public class NotificationHub:Hub
    {
        private static readonly Dictionary<string, string> OnlineUsers = new();

        public override Task OnConnectedAsync()
        {
            var userId = Context.User?.Claims
        .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                OnlineUsers[userId] = Context.ConnectionId;
                Console.WriteLine(OnlineUsers);
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userId))
            {
                OnlineUsers.Remove(userId);
            }
            return base.OnDisconnectedAsync(exception);
        }
        public static string? IsUserOnline(string userId)
        {
            return OnlineUsers.TryGetValue(userId, out var connectionId) ? connectionId : null;
        }

    }
}
