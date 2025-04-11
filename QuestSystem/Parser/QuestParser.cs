using System.Text.Json;
using QuestSystem.Parser.Dtos;

namespace QuestSystem.Parser;

public static class QuestParser {

    private static JsonSerializerOptions _options = new JsonSerializerOptions  {
        PropertyNameCaseInsensitive = true
    };

    
    public static QuestDto? Parse(string questJson) {
        return JsonSerializer.Deserialize<QuestDto>(questJson);
    }
    
}