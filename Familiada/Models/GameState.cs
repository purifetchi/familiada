using System.Text.Json.Serialization;

namespace Familiada.Models;

public class GameState
{
    public required Guid Id { get; set; }
    public string? HostId { get; set; }
    public GameSchema? Schema { get; set; }
    public int Multiplier { get; set; } = 1;
    public int RoundIndex { get; set; } = 0;
    
    public Dictionary<Team, TeamInfo> Teams { get; set; } = new Dictionary<Team, TeamInfo>();
    public HashSet<int> AnsweredQuestions { get; set; } = new HashSet<int>();

    [JsonIgnore]
    public RoundSchema? CurrentRound => Schema?.Rounds[RoundIndex];

    [JsonIgnore]
    public int Points => AnsweredQuestions.Select(i => CurrentRound!.Answers[i].Points).Sum() * Multiplier;
}