namespace Familiada.Services;

public class GameSessionService
{
    public async Task<Guid> CreateSession()
    {
        var guid = Guid.NewGuid();

        return guid;
    }

    public async Task BindOperatorToSession(Guid sessionId, string connectionId)
    {
        // TODO
    }
}