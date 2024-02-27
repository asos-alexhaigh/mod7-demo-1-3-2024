using System.Reflection.Emit;
using System.Text.Json.Nodes;
using System.Text.Json;
using ParkYourLark.WebApi.Data;

namespace ParkYourLark.WebApi
{
    public class RequestParser : IRequestParser
    {
        public LevelSpace Parse(dynamic value)
        {
            JsonNode data = JsonSerializer.Deserialize<JsonNode>(value);
            string levelId = (string)data["Level"];
            string spaceId = (string)data["Space"];

            return new LevelSpace() { Level = new Level { Id = levelId }, Space = spaceId };
        }
    }
}
