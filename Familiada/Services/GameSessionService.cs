using Familiada.Models;

namespace Familiada.Services;

public class GameSessionService(GameStoreService gameStoreService)
{
    public async Task<Guid> CreateSession()
    {
        var state = await gameStoreService.CreateGameState();
        return state.Id;
    }

    public async Task BindOperatorToSession(Guid sessionId, string connectionId)
    {
        await gameStoreService.EditGameState(sessionId, state =>
        {
            state.HostId = connectionId;
        });
    }

    public async Task SetGameSchema(Guid sessionId, GameSchema schema)
    {
        await gameStoreService.EditGameState(sessionId, state =>
        {
            state.Schema = schema;
            state.RoundIndex = 0;
        });
    }
    
    public async Task SetTeamNames(Guid sessionId, string leftTeamName, string rightTeamName)
    {
        await gameStoreService.EditGameState(sessionId, state =>
        {
            state.Teams[Team.Left].Name = leftTeamName;
            state.Teams[Team.Right].Name = rightTeamName;
        });
    }

    public async Task AddAnswerToPointPool(Guid sessionId, int answerIndex)
    {
        await gameStoreService.EditGameState(sessionId, state =>
        {
            state.AnsweredQuestions.Add(answerIndex);
        });
    }

    public async Task EndRound(Guid sessionId)
    {
        await gameStoreService.EditGameState(sessionId, state =>
        {
            state.AnsweredQuestions = [];
            
            state.RoundIndex++;
            state.Multiplier = state.RoundIndex switch
            {
                0 or 1 => 1,
                2 => 2,
                _ => 3
            };
        });
    }

    public async Task AwardPoints(Guid sessionId, Team team)
    {
        await gameStoreService.EditGameState(sessionId, state =>
        {
            state.Teams[team].Points += state.Points;
            state.AnsweredQuestions = [];
        });
    }
}