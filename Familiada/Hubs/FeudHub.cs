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
        
        var shouldEndRound = await gameSessionService.AddAnswerToPointPool(GetSessionId(), index);
        if (shouldEndRound)
            await EndRound();
    }

    public async Task SendWrong(WrongType type)
    {
        var state = await gameStoreService.GetStateForId(GetSessionId());
        await Clients.All.SendAsync("WrongAnswer", state!.PointsWinningTeam, type);

        var shouldEndRound = await gameSessionService.IncrementWrongForTeam(GetSessionId(), state!.PointsWinningTeam, type);
        
        var newState = await gameStoreService.GetStateForId(GetSessionId());
        await Clients.All.SendAsync("SetCurrentRoundPointWinningTeam", new PointWinningTeamUpdate
        {
            WinningTeam = (int)newState!.PointsWinningTeam,
            CanSendBigWrongAnswers = true,
            CanSendSmallWrongAnswers = newState.PointsWinningTeam == state.PointsWinningTeam
        });

        if (shouldEndRound)
            await EndRound();
    }
    
    public async Task PrepareGame(string roundJson)
    {
        var gameSchema = JsonSerializer.Deserialize<GameSchema>(roundJson);
        await gameSessionService.SetGameSchema(GetSessionId(), gameSchema!);

        var state = await gameStoreService.GetStateForId(GetSessionId());
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

        await SendTeamUpdate();
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
    }

    public async Task SetStartingTeam(Team team)
    {
        await gameSessionService.SetPointWinningTeam(GetSessionId(), team);
        await Clients.All.SendAsync("SetCurrentRoundPointWinningTeam", new PointWinningTeamUpdate
        {
            WinningTeam = (int)team,
            CanSendBigWrongAnswers = true,
            CanSendSmallWrongAnswers = true
        });
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