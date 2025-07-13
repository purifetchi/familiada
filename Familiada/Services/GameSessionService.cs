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

    public async Task<bool> IncrementWrongForTeam(Guid sessionId, Team team, WrongType wrongType)
    {
        var shouldEndRound = false;
        await gameStoreService.EditGameState(sessionId, state =>
        {
            var teamInfo = state!.Teams[team];
            teamInfo.IncrementWrongCounter(wrongType);
        
            if (teamInfo.IsOut)
            {
                state.PointsWinningTeam = team switch
                {
                    Team.Left => Team.Right,
                    Team.Right => Team.Left,
                    _ => throw new InvalidOperationException()
                };

                shouldEndRound = state.Teams[state.PointsWinningTeam].IsOut;
            }
        });

        return shouldEndRound;
    }

    public async Task<bool> AddAnswerToPointPool(Guid sessionId, int answerIndex)
    {
        var shouldEndRound = false;
        await gameStoreService.EditGameState(sessionId, state =>
        {
            state.AnsweredQuestions.Add(answerIndex);
            state.Points += state.CurrentRound!.Answers[answerIndex].Points * state.Multiplier;
            
            shouldEndRound = state.AnsweredQuestions.Count == state.CurrentRound!.Answers.Length;
        });
        
        return shouldEndRound;
    }

    public async Task SetPointWinningTeam(Guid sessionId, Team team)
    {
        await gameStoreService.EditGameState(sessionId, state =>
        {
            state.PointsWinningTeam = team;
        });
    }

    public async Task EndRound(Guid sessionId)
    {
        await gameStoreService.EditGameState(sessionId, state =>
        {
            state.Teams[state.PointsWinningTeam].Points += state.Points;
            state.Points = 0;
            state.AnsweredQuestions = new HashSet<int>();
            
            state.RoundIndex++;
            state.Multiplier++;

            foreach (var team in state.Teams)
            {
                team.Value.Reset();
            }
        });
    }
}