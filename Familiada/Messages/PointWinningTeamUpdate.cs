using System.Text.Json.Serialization;

namespace Familiada.Messages;

public class PointWinningTeamUpdate
{
    [JsonPropertyName("winningTeam")]
    public int WinningTeam { get; set; }
    
    [JsonPropertyName("canSendSmallWrongAnswers")]
    public bool CanSendSmallWrongAnswers { get; set; }

    [JsonPropertyName("canSendBigWrongAnswers")]
    public bool CanSendBigWrongAnswers { get; set; }
}