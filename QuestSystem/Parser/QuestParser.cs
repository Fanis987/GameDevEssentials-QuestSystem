using System.Text.Json;
using QuestSystem.Entities;
using QuestSystem.Parser.Dtos;

namespace QuestSystem.Parser;

public static class QuestParser {

    private static JsonSerializerOptions? _options = new JsonSerializerOptions  {
        PropertyNameCaseInsensitive = true
    };
    
    public static List<Quest> LoadFromJson(string json, JsonSerializerOptions? options = null)
    {
        // Basic Checks
        if(string.IsNullOrEmpty(json)) throw new ArgumentException("Cannot parse an empty json", nameof(json));
        if(!json.StartsWith("{") && !json.StartsWith("[")) throw new ArgumentException("Json should start with '{' or '['", nameof(json));
        options ??= _options;
        var questList = new List<Quest>();
        
        // Parse the json
        if (json.StartsWith("{") && json.EndsWith("}")) //SINGLE QUEST
        {
            var questDto = JsonSerializer.Deserialize<QuestDto>(json,options);
            if(questDto == null) return new List<Quest>();
            questList.Add(questDto.ToQuest());
            return questList;
        }
        
        //QUEST ARRAY
        var questDtos = JsonSerializer.Deserialize<List<QuestDto>>(json,options);
        if(questDtos == null) return new List<Quest>();
        
        // Check parsed data
        foreach (var questDto in questDtos) {
            var result = IsValidDto(questDto);
            if(!result.IsSuccessful) continue;
            questList.Add(questDto.ToQuest());
        }
        
        return questList;
    }
    
    //Todo: make internal
    internal static ParseResult IsValidDto(QuestDto questDto)
    {
        var pre = $"Error when parsing the quest id {questDto.Id}: ";
        
        // Quest Tests
        if(questDto.Stages.Count == 0) return ParseResult.Fail(pre + "No Stages Found");
        
        //Stage tests
        foreach (var stage in questDto.Stages) {
            //stages should not be completed when reading from json file
            if(stage.IsCompleted) return ParseResult.Fail(pre + "Completed stage found");
            //objectives test
            if(stage.Objectives.Count == 0) return ParseResult.Fail(pre + "No Objectives Found");
        }
        
        return ParseResult.Ok();
    }
    
    
}