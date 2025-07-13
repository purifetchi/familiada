using System.Text.Json.Serialization;

namespace Familiada.Models;

public class UpcomingRound
{
    [JsonPropertyName("question")]
    public string Question { get; set; }
    
    [JsonPropertyName("multiplier")]
    public int Multiplier { get; set; }
}