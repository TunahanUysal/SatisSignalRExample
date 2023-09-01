using Microsoft.AspNetCore.SignalR;

namespace SatisSignalRExample.Hubs
{
    public class SatisHub:Hub
    {
        public async Task SendMessage()
        {
            await Clients.All.SendAsync("receiveMessage", "Merhaba");
        }

    }
}
