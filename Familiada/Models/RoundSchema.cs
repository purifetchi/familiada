using System.Text.Json.Serialization;

namespace Familiada.Models;

public class RoundSchema
{
    [JsonPropertyName("question")]
    public string Question { get; set; } = null!;
    
    [JsonPropertyName("answers")]
    public AnswerSchema[] Answers { get; set; } = [];
}
