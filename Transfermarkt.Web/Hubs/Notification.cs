using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Transfermarkt.Web.Hubs
{
    public class Notification : Hub
    {
        public async Task Send(string message)
        {
            //await Clients.All.SendAsync("ReceiveMessage", user, message);
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
        
    }
}
