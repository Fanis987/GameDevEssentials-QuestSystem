using System.Text.Json;
using QuestSystem.Entities;
using QuestSystem.Parser.Dtos;

namespace QuestSystem.Parser;

/// <summary>
/// Helper static class for parsing json to quest objects
/// </summary>
public static class QuestParser {

    private static JsonSerializerOptions? _options = new JsonSerializerOptions  {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Parses a json in a specified path into a list of quests.
    /// The validity of the json structure is also checked.
    /// Invalid quests will NOT be included in the returned list.
    /// <param name="jsonPath">The string containing the json</param>
    /// <param name="jsonOptions">The list of valid quests parsed from the json</param>
    /// <returns></returns>
    /// </summary>
    public static List<Quest>? LoadFromJsonFile(string jsonPath, JsonSerializerOptions? jsonOptions = null)
    {
        try
        {
            var json = File.ReadAllText(jsonPath);
            return LoadFromJson(json, jsonOptions);
        }
        catch(Exception e)
        {
            return null;
        }
    }
    
    /// <summary>
    /// Parses a json string into a list of quests.
    /// The validity of the structure is also  checked.
    /// Invalid quests will NOT be included in the returned list.
    /// </summary>
    /// <param name="json">The string containing the json</param>
    /// <param name="options">Json options for the serialization</param>
    /// <returns>The list of valid quests parsed from the json</returns>
    /// <exception cref="ArgumentException"></exception>
    internal static List<Quest> LoadFromJson(string json, JsonSerializerOptions? options = null)
    {
        // Basic Checks
        if(string.IsNullOrEmpty(json)) throw new ArgumentException("Cannot parse an empty json", nameof(json));
        if(!json.StartsWith("{") && !json.StartsWith("[")) throw new ArgumentException("Json should start with '{' or '['", nameof(json));
        options ??= _options;
        var questList = new List<Quest>();
        
        // Parse the json
        if (json.StartsWith('{') && json.EndsWith('}')) //SINGLE QUEST
        {
            try {
                var questDto = JsonSerializer.Deserialize<QuestDto>(json, options);
                if (questDto == null) return new List<Quest>();
                questList.Add(questDto.ToQuest());
                return questList;
            }
            catch (JsonException jsonEx) {
                return questList;
            }
        }
        
        //QUEST ARRAY
        List<QuestDto>? questDtos;
        try {
            questDtos = JsonSerializer.Deserialize<List<QuestDto>>(json,options);
        }
        catch (JsonException jsonEx) {
            return questList;
        }
        if(questDtos == null) return new List<Quest>();
        
        // Check parsed data
        foreach (var questDto in questDtos) {
            var result = IsValidDto(questDto);
            if(!result.IsSuccessful) continue;
            questList.Add(questDto.ToQuest());
        }
        
        return questList;
    }
    
    /// <summary>
    /// Checks the validity of a parsed quest.
    /// </summary>
    /// <param name="questDto">The questDto extracted from a json</param>
    /// <returns>A <see cref="ParseResult"/> with the result of the evaluation.></returns>
    internal static ParseResult IsValidDto(QuestDto questDto)
    {
        var pre = $"Error when parsing the quest id {questDto.Id}: ";
        
        // Quest Tests
        if(questDto.Id <= 0 ) return ParseResult.Fail(pre + "ID is required, and should be positive integer");
        if(String.IsNullOrEmpty(questDto.Title)) return ParseResult.Fail(pre + "Must have a title");
        if(questDto.Stages.Count == 0) return ParseResult.Fail(pre + "No Stages Found");
        
        //Stage tests
        foreach (var stage in questDto.Stages) {
            //stages should not be completed when reading from json file
            if(stage.IsCompleted) return ParseResult.Fail(pre + "Completed stage found");
            //objectives test
            if(stage.Objectives.Count == 0) return ParseResult.Fail(pre + "No Objectives Found");
            foreach (var objective in stage.Objectives) {
                if(objective.GoalValue == 0) return ParseResult.Fail(pre + "Goal value of 0 found");
            }
        }
        return ParseResult.Ok();
    }
}