using System.Text.Json.Serialization;

namespace Familiada.Models;

public class GameSchema
{
    [JsonPropertyName("rounds")]
    public RoundSchema[] Rounds { get; set; } = [];
}