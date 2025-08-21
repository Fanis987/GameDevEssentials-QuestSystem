using System.Text.Json;
using QuestSystem.Entities;
using QuestSystem.Parser.Dtos;
using QuestSystem.Parser.Util;

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
    public static MultiParseResult LoadFromJsonFile(string jsonPath, JsonSerializerOptions? jsonOptions = null)
    {
        try {
            var json = File.ReadAllText(jsonPath);
            return LoadFromJson(json, jsonOptions);
        }
        catch(Exception e) {
            return new MultiParseResult();
        }
    }
    
    /// <summary>
    /// Parses a json string into a list of quests.
    /// The validity of the structure is also checked.
    /// Invalid quests will NOT be included in the returned list.
    /// </summary>
    /// <param name="json">The string containing the json</param>
    /// <param name="options">Json options for the serialization</param>
    /// <returns>The list of valid quests parsed from the json</returns>
    /// <exception cref="ArgumentException"></exception>
    internal static MultiParseResult LoadFromJson(string json, JsonSerializerOptions? options = null)
    {
        // Basic Checks
        if(string.IsNullOrEmpty(json)) throw new ArgumentException("Cannot parse an empty json", nameof(json));
        if(!json.StartsWith("{") && !json.StartsWith("[")) throw new ArgumentException("Json should start with '{' or '['", nameof(json));
        options ??= _options;
        
        // Parse the json
        var parseResult = new MultiParseResult();
        if (json.StartsWith('{') && json.EndsWith('}')) //SINGLE QUEST
        {
            try {
                var questDto = JsonSerializer.Deserialize<QuestDto>(json, options);
                if (questDto == null) return new MultiParseResult();
                //check questDto
                var result = IsValidQuestDto(questDto);
                if (!result.IsSuccessful) {
                    parseResult.ErrorMessages.Add(result.ErrorMessage);
                    return parseResult;
                }
                parseResult.Quests.Add(questDto.ToQuest());
                return parseResult;
            }
            catch (JsonException jsonEx) {
                parseResult.ErrorMessages.Add($"A JsonException occured: "+ jsonEx.Message);
                return parseResult;
            }
        }
        
        //QUEST ARRAY
        List<QuestDto>? questDtos;
        try {
            questDtos = JsonSerializer.Deserialize<List<QuestDto>>(json,options);
        }
        catch (JsonException jsonEx) {
            parseResult.ErrorMessages.Add($"A JsonException occured"+ jsonEx.Message);
            return parseResult;
        }
        if(questDtos == null) return new MultiParseResult();
        
        // Check parsed data
        foreach (var questDto in questDtos) {
            var result = IsValidQuestDto(questDto);
            if (!result.IsSuccessful) {
                parseResult.ErrorMessages.Add(result.ErrorMessage);
                continue;
            }
            parseResult.Quests.Add(questDto.ToQuest());
        }
        return parseResult;
    }
    
    /// <summary>
    /// Checks the validity of a parsed quest dto.
    /// </summary>
    /// <param name="questDto">The questDto extracted from a json</param>
    /// <returns>A <see cref="ParseResult"/> with the result of the evaluation.></returns>
    internal static ParseResult IsValidQuestDto(QuestDto questDto)
    {
        var pre = $"Error when parsing the quest id {questDto.Id}: ";
        
        // Quest Tests
        if(questDto.Id <= 0 ) return ParseResult.Fail(pre + "Quest ID is required, and should be a positive integer");
        if(string.IsNullOrEmpty(questDto.Title)) return ParseResult.Fail(pre + "Must have a title");
        if(questDto.Title.Length > Quest.TitleCharLimit) return ParseResult.Fail(pre + "Title must be limited to 200 chars");
        if(questDto.Stages.Count == 0) return ParseResult.Fail(pre + "No Stages Found in quest");
        
        //Stage tests
        foreach (var stageDto in questDto.Stages) {
            //stages should not be completed when reading from json file
            if(stageDto.Id < 0 ) return ParseResult.Fail(pre + "Stage ID is required, should be positive integer");
            if(stageDto.IsCompleted) return ParseResult.Fail(pre + "Completed stage found");
            if(string.IsNullOrEmpty(stageDto.Description)) return ParseResult.Fail(pre + "Stages must have a description");
            if(stageDto.Description.Length > Quest.DescriptionCharLimit) return ParseResult.Fail(pre + "Stage description must be limited to 2000 chars");
            if(stageDto.PathDtos.Count == 0) return ParseResult.Fail(pre + "No paths Found");
            
            //stage path tests
            foreach (var stagePathDto in stageDto.PathDtos) {
                if(stagePathDto.Objectives.Count == 0) return ParseResult.Fail(pre + "No Objectives Found");
                foreach (var objective in stagePathDto.Objectives) {
                    if(objective.GoalValue <= 0) return ParseResult.Fail(pre + "Goal value must be positive");
                    if(objective.TaskTypeId <= 0) return ParseResult.Fail(pre + "Task Id value must be positive");
                }
            }
            
        }
        return ParseResult.Ok();
    }
}