using Microsoft.AspNetCore.SignalR;

namespace Familiada.Hubs;

public class FeudHub : Hub
{
    public async Task Hello()
    {
        
    }

    public async Task HelloOperator(string token)
    {
        await Clients.All.SendAsync("OperatorConnected", token);
    }
}