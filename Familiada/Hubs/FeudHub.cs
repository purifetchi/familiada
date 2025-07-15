using System.Text.Json;
using Familiada.Messages;
using Familiada.Models;
using Familiada.Services;
using Microsoft.AspNetCore.SignalR;

namespace Familiada.Hubs;

public class FeudHub(
    GameSessionService gameSessionService,
    GameStoreService gameStoreService) : Hub
{
    private void SetSessionId(Guid sessionId)
    {
        Context.Items["sessionId"] = sessionId;
    }

    private Guid GetSessionId()
    {
        return Guid.Parse(Context.Items["sessionId"]!.ToString()!);
    }
    
    public async Task Hello()
    {
        var sessionId = await gameSessionService.CreateSession();
        SetSessionId(sessionId);
        
        await Clients.Caller.SendAsync("SessionCreated", sessionId);
    }

    public async Task HelloHost(Guid sessionId)
    {
        await gameSessionService.BindOperatorToSession(sessionId, Context.ConnectionId);
        SetSessionId(sessionId);
        
        await Clients.All.SendAsync("HostConnected", sessionId);
    }

    public async Task SendAnswer(int index)
    {
        var state = await gameStoreService.GetStateForId(GetSessionId());
        var answer = state!.CurrentRound!.Answers[index];
        
        await Clients.All.SendAsync("ShowPanelAnswer", index, answer.Text, answer.Points);
        
        await gameSessionService.AddAnswerToPointPool(GetSessionId(), index);
    }

    public async Task SendWrong(Team team, WrongType type)
    {
        await Clients.All.SendAsync("WrongAnswer", team, type);
    }
    
    public async Task ClearWrong(Team team)
    {
        await Clients.All.SendAsync("ClearWrongDisplay", team);
    }
    
    public async Task AwardPoints(Team team)
    {
        await gameSessionService.AwardPoints(GetSessionId(), team);
        await SendTeamUpdate();
    }
    
    public async Task PrepareGame(string roundJson, string leftTeamName, string rightTeamName)
    {
        var gameSchema = JsonSerializer.Deserialize<GameSchema>(roundJson);
        await gameSessionService.SetGameSchema(GetSessionId(), gameSchema!);

        await gameSessionService.SetTeamNames(GetSessionId(), leftTeamName, rightTeamName);
        
        var state = await gameStoreService.GetStateForId(GetSessionId());

        await Clients.All.SendAsync("EndRound");
        await Clients.Caller.SendAsync("ShowUpcomingRound", new UpcomingRound
        {
            Question = state!.CurrentRound!.Question,
            Multiplier = state.Multiplier
        });
    }

    public async Task EndRound()
    {
        await gameSessionService.EndRound(GetSessionId());
        
        var state = await gameStoreService.GetStateForId(GetSessionId());
        await Clients.Caller.SendAsync("ShowUpcomingRound", new UpcomingRound
        {
            Question = state!.CurrentRound!.Question,
            Multiplier = state.Multiplier
        });

        await Clients.All.SendAsync("EndRound");
    }
    
    public async Task StartRound()
    {
        var state = await gameStoreService.GetStateForId(GetSessionId());

        await Clients.All.SendAsync("SetRoundInfo", new RoundInfo()
        {
            QuestionAmount = state!.CurrentRound!.Answers.Length,
            Multiplier = state.Multiplier
        });

        await Clients.All.SendAsync("SetHostRoundSchema", state.CurrentRound!);

        await SendTeamUpdate();
    }

    private async Task SendTeamUpdate()
    {
        var state = await gameStoreService.GetStateForId(GetSessionId());
        List<TeamUpdate> teamUpdates =
        [
            TeamUpdate.FromTeamInfo(state!.Teams[Team.Left]),
            TeamUpdate.FromTeamInfo(state!.Teams[Team.Right]),
        ];
        
        await Clients.All.SendAsync("SetTeamData", teamUpdates);
    }
}