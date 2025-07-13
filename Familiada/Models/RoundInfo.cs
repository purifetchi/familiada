using System.Text.Json.Serialization;

namespace Familiada.Models;

public class RoundInfo
{
    [JsonPropertyName("questionAmount")]
    public int QuestionAmount { get; set; }
    
    [JsonPropertyName("multiplier")]
    public int Multiplier { get; set; }
}