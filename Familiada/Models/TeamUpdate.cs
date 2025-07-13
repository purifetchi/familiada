using System.Text.Json.Serialization;

namespace Familiada.Models;

public class TeamUpdate
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    [JsonPropertyName("points")]
    public required int Points { get; set; }

    public static TeamUpdate FromTeamInfo(TeamInfo teamInfo)
    {
        return new TeamUpdate()
        {
            Name = teamInfo.Name,
            Points = teamInfo.Points
        };
    }
}