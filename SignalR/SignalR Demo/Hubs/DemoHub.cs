using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR_Demo.Hubs
{
    public class DemoHub : Hub
    {
        public Task SendMessage(string user, string message)
        {
            Console.WriteLine($"SendMessage was called by a client: User={user} Message={message}");
            return Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
