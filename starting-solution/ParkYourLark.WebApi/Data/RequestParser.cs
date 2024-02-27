using System.Text.Json.Nodes;
using System.Text.Json;

namespace ParkYourLark.WebApi.Data
{
    // RequestParser.cs
    public class RequestParser : IRequestParser
    {
        //public LevelSpace Parse(dynamic value)
        //{
        //    JsonNode data = JsonSerializer.Deserialize<JsonNode>(value);
        //    string levelId = (string)data["Level"];
        //    string spaceId = (string)data["Space"];

        //    return new LevelSpace() { Level = new Level { Id = levelId }, Space = spaceId };
        //}
    }

    // IRequestParser.cs
    public interface IRequestParser
    {
        //LevelSpace Parse(dynamic value);
    }
}
