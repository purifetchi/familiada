using System.Text.Json;
using Familiada.Models;
using StackExchange.Redis;

namespace Familiada.Services;

public class GameStoreService(IConnectionMultiplexer connectionMultiplexer)
{
    /// <summary>
    /// The Redis connection.
    /// </summary>
    private IDatabase _db = connectionMultiplexer.GetDatabase();
    
    public async Task<GameState> CreateGameState()
    {
        GameState? gameState = null;
        do
        {
            var guid = Guid.NewGuid();
            if (await _db.KeyExistsAsync(guid.ToString()))
                continue;

            gameState = new GameState
            {
                Id = guid,
                Teams = new Dictionary<Team, TeamInfo>()
                {
                    {
                        Team.Left,
                        new TeamInfo()
                    },
                    {
                        Team.Right,
                        new TeamInfo()
                    }
                }
            };

            await _db.StringSetAsync(guid.ToString(), JsonSerializer.Serialize(gameState));
        } while (gameState == null);
        
        return gameState;
    }

    public async Task<GameState?> GetStateForId(Guid id)
    {
        var value = await _db.StringGetAsync(id.ToString());
        if (!value.HasValue)
            return null;
        
        return JsonSerializer.Deserialize<GameState>(value!);
    }

    public async Task SetStateForId(Guid id, GameState gameState)
    {
        var value = await _db.StringGetAsync(id.ToString());
        if (!value.HasValue)
            return;
        
        await _db.StringSetAsync(id.ToString(), JsonSerializer.Serialize(gameState));
    }
    
    public async Task EditGameState(Guid id, Action<GameState> editor)
    {
        // TODO: Async locking.
        var state = await GetStateForId(id);
        if (state == null)
            return;

        editor(state);
        await SetStateForId(id, state);
    }
}