using System.Text.Json.Serialization;

namespace Familiada.Models;

public class TeamInfo
{
    public int Points { get; set; } = 0;
    public string Name { get; set; } = "DRUZYNA";
    
    public int SmallWrongCounter { get; set; } = 0;
    public int BigWrongCounter { get; set; } = 0;

    public void IncrementWrongCounter(WrongType type)
    {
        switch (type)
        {
            case WrongType.Small:
                SmallWrongCounter++;
                break;
            case WrongType.Big:
                BigWrongCounter++;
                break;
        }
    }

    public void Reset()
    {
        SmallWrongCounter = 0;
        BigWrongCounter = 0;
    }
    
    [JsonIgnore]
    public bool IsOut => SmallWrongCounter == 3 || BigWrongCounter == 1;
}