using Familiada.Models;
using Familiada.Services;
using Microsoft.AspNetCore.SignalR;

namespace Familiada.Hubs;

public class FeudHub(
    GameSessionService gameSessionService) : Hub
{
    public async Task Hello()
    {
        var sessionId = await gameSessionService.CreateSession();
        await Clients.Caller.SendAsync("SessionCreated", sessionId);
    }

    public async Task HelloHost(Guid sessionId)
    {
        await gameSessionService.BindOperatorToSession(sessionId, Context.ConnectionId);
        await Clients.All.SendAsync("HostConnected", sessionId);
    }

    public async Task SendAnswer(int index)
    {
        await Clients.All.SendAsync("CorrectAnswer", index, "ZAPISKI ZIELARKI", 35);
    }

    public async Task SendWrong(Team team, WrongType type)
    {
        await Clients.All.SendAsync("WrongAnswer", team, type);
    }
}