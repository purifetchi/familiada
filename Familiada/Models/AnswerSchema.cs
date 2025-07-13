using System.Text.Json.Serialization;

namespace Familiada.Models;

public class AnswerSchema
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = null!;
    
    [JsonPropertyName("points")]
    public int Points { get; set; }
}
