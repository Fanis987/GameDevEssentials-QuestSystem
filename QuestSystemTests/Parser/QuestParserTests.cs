using System.Text.Json;
using QuestSystem.Parser;
using QuestSystem.Parser.Dtos;

namespace QuestSystemTests.Parser;

public class QuestParserTests
{
    [Fact]
    public void LoadFromJson_CanParseSmallJson() {
        var result = QuestParser.LoadFromJson(QuestJsons.SmallQuestJson);
        Assert.NotNull(result);
        
        var questList = result.Quests;
        Assert.NotNull(questList);
        Assert.Single(questList);
        
        var quest = questList[0];
        Assert.NotNull(quest);
        Assert.Equal(1, quest.Id);
        Assert.Equal(2, quest.NextQuestId);
        Assert.Equal("First Quest", quest.Title);
        Assert.True(quest.IsMainQuest);
        Assert.False(quest.IsCompleted);
        Assert.False(quest.WasFailed);

        var stage = quest.CurrentStage;
        Assert.Equal("This is stage 1", stage.StageDescription);
        Assert.False(stage.IsCompleted);
    }
    
    [Fact]
    public void LoadFromJson_CanDetectJsonErrors() {
        var multiParseResult = QuestParser.LoadFromJson(QuestJsons.StringAtIntPosJson);
        Assert.NotNull(multiParseResult);
        
        var questList = multiParseResult.Quests;
        Assert.NotNull(questList);
        Assert.Empty(questList);

        var errorList = multiParseResult.ErrorMessages;
        Assert.NotNull(errorList);
        Assert.Single(errorList);
    }
    
    [Fact]
    public void LoadFromJson_CanDetectJsonErrors2() {
        var multiParseResult = QuestParser.LoadFromJson(QuestJsons.NumAtBoolPosJson);
        Assert.NotNull(multiParseResult);
        
        var questList = multiParseResult.Quests;
        Assert.NotNull(questList);
        Assert.Empty(questList);

        var errorList = multiParseResult.ErrorMessages;
        Assert.NotNull(errorList);
        Assert.Single(errorList);
    }
    
     [Fact]
     public void IsValidDto_ShouldReturnOkForValidQuestDto() {
         var questDto = JsonSerializer.Deserialize<QuestDto>(QuestJsons.SmallQuestJson);
         Assert.NotNull(questDto);
         var result = QuestParser.IsValidQuestDto(questDto);

         Assert.True(result.IsSuccessful);
         Assert.Equal(string.Empty, result.ErrorMessage);
     }
        
     
     [Fact]
     public void IsValidDto_ShouldReturnOkForValidQuestDto2()
     {
         var questDto = JsonSerializer.Deserialize<QuestDto>(QuestJsons.MediumQuestJson);
         Assert.NotNull(questDto);

         var result = QuestParser.IsValidQuestDto(questDto);
         Assert.True(result.IsSuccessful);
         Assert.Equal(string.Empty, result.ErrorMessage);

         var quest = questDto.ToQuest();
         Assert.NotNull(quest);
     }

     
     [Fact]
     public void IsValidDto_ShouldFailWhenNoStagesPresent()
     {
         var questDto = JsonSerializer.Deserialize<QuestDto>(QuestJsons.NoStagesJson);
         Assert.NotNull(questDto);
         var result = QuestParser.IsValidQuestDto(questDto);

         Assert.False(result.IsSuccessful);
         Assert.Contains("No Stages Found", result.ErrorMessage);
     }
     
     [Fact]
     public void IsValidDto_ShouldFailWithoutId() {
         var questDto = JsonSerializer.Deserialize<QuestDto>(QuestJsons.QuestNoIdJson);
         Assert.NotNull(questDto);
         var result = QuestParser.IsValidQuestDto(questDto);

         Assert.False(result.IsSuccessful);
         Assert.Contains("ID is required", result.ErrorMessage);
     }
     
     [Fact]
     public void IsValidDto_ShouldFailWithoutTitle() {
         var questDto = JsonSerializer.Deserialize<QuestDto>(QuestJsons.QuestNoTitleJson);
         Assert.NotNull(questDto);
         var result = QuestParser.IsValidQuestDto(questDto);

         Assert.False(result.IsSuccessful);
         Assert.Contains("Must have a title", result.ErrorMessage);
     }
     
     [Fact]
     public void IsValidDto_ShouldFailWithLongTitle() {
         var questDto = JsonSerializer.Deserialize<QuestDto>(QuestJsons.QuestLongTitleJson);
         Assert.NotNull(questDto);
         var result = QuestParser.IsValidQuestDto(questDto);

         Assert.False(result.IsSuccessful);
         Assert.Contains("Title must be limited", result.ErrorMessage);
     }
     
     
     [Fact]
     public void IsValidDto_ShouldFailWhenStageIsCompleted()
     {
         var questDto = JsonSerializer.Deserialize<QuestDto>(QuestJsons.CompletedStageJson);
         Assert.NotNull(questDto);
         var result = QuestParser.IsValidQuestDto(questDto);

         Assert.False(result.IsSuccessful);
         Assert.Contains("Completed stage found", result.ErrorMessage);
     }

     [Fact]
     public void IsValidDto_ShouldFail_WhenStageHasNoObjectives()
     {
         var questDto = JsonSerializer.Deserialize<QuestDto>(QuestJsons.StageWithNoObjectivesJson);
         Assert.NotNull(questDto);
         var result = QuestParser.IsValidQuestDto(questDto);

         Assert.False(result.IsSuccessful);
         Assert.Contains("No Objectives Found", result.ErrorMessage);
     }
     
     [Fact]
     public void IsValidDto_ShouldFail_WhenStageHasZeroGoalValue()
     {
         var questDto = JsonSerializer.Deserialize<QuestDto>(QuestJsons.QuestNoGoal);
         Assert.NotNull(questDto);
         var result = QuestParser.IsValidQuestDto(questDto);

         Assert.False(result.IsSuccessful);
         Assert.Contains("Goal value must be positive", result.ErrorMessage);
     }
     
     [Fact]
     public void IsValidDto_ShouldFail_WhenStageHasNegativeGoalValue()
     {
         var questDto = JsonSerializer.Deserialize<QuestDto>(QuestJsons.QuestNegativeGoal);
         Assert.NotNull(questDto);
         var result = QuestParser.IsValidQuestDto(questDto);

         Assert.False(result.IsSuccessful);
         Assert.Contains("Goal value must be positive", result.ErrorMessage);
     }     
     
     [Fact]
     public void IsValidDto_ShouldFail_WhenStageHasNegativeTAskIdValue()
     {
         var questDto = JsonSerializer.Deserialize<QuestDto>(QuestJsons.QuestNegativeTAskId);
         Assert.NotNull(questDto);
         var result = QuestParser.IsValidQuestDto(questDto);

         Assert.False(result.IsSuccessful);
         Assert.Contains("Task Id value must be positive", result.ErrorMessage);
         
     }
     
     [Fact]
     public void IsValidDto_ShouldFail_WhenStageIdIsNegative()
     {
         var questDto = JsonSerializer.Deserialize<QuestDto>(QuestJsons.StageWithNegativeIdJson);
         Assert.NotNull(questDto);
         var result = QuestParser.IsValidQuestDto(questDto);

         Assert.False(result.IsSuccessful);
         Assert.Contains("Stage ID is required", result.ErrorMessage);
     }
     
     [Fact]
     public void IsValidDto_ShouldFail_WhenStageHasNoPathDtos()
     {
         var questDto = JsonSerializer.Deserialize<QuestDto>(QuestJsons.StageWithNoPathDtosJson);
         Assert.NotNull(questDto);
         var result = QuestParser.IsValidQuestDto(questDto);

         Assert.False(result.IsSuccessful);
         Assert.Contains("No paths Found", result.ErrorMessage);
     }
     
     [Fact]
     public void IsValidDto_ShouldFail_WhenStageHasNoDescription()
     {
         var questDto = JsonSerializer.Deserialize<QuestDto>(QuestJsons.StageWithNoDescriptionJson);
         Assert.NotNull(questDto);
         var result = QuestParser.IsValidQuestDto(questDto);

         Assert.False(result.IsSuccessful);
         Assert.Contains("must have a description", result.ErrorMessage);
     }
     
     [Fact]
     public void IsValidDto_ShouldFail_WhenStageHasLongDescription()
     {
         var questDto = JsonSerializer.Deserialize<QuestDto>(QuestJsons.QuestLongDescriptionJson);
         Assert.NotNull(questDto);
         var result = QuestParser.IsValidQuestDto(questDto);

         Assert.False(result.IsSuccessful);
         Assert.Contains("Stage description must be limited", result.ErrorMessage);
     }
     
     [Fact]
     public void LoadFromJson_CannotParseEmptyAndInvalidJson() {
        Assert.Throws<ArgumentException>(() => QuestParser.LoadFromJson(QuestJsons.EmptyJson));
        Assert.Throws<ArgumentException>(() => QuestParser.LoadFromJson(null));
        Assert.Throws<ArgumentException>(() => QuestParser.LoadFromJson(QuestJsons.NumberJson));
        Assert.Throws<ArgumentException>(() => QuestParser.LoadFromJson(QuestJsons.SpecialCharJson));
     }
     
     [Fact]
     public void LoadFromJson_CannotParseInvalidJson() {
         var parseResult = QuestParser.LoadFromJson(QuestJsons.QuestInvalidId);
         var questList = parseResult.Quests;
         var errors = parseResult.ErrorMessages;
         
         var parseResult2 = QuestParser.LoadFromJson(QuestJsons.QuestListInvalidId);
         var questList2 = parseResult2.Quests;
         var errors2 = parseResult.ErrorMessages;
         
         Assert.NotNull(questList);
         Assert.Empty(questList);
         Assert.Single(errors);
         Assert.NotNull(questList2);
         Assert.Empty(questList2);
         Assert.Single(errors2);
     }

     [Fact]
     public void LoadFromJson_CannotParseNoStageJson()
     {
         var parseResult = QuestParser.LoadFromJson(QuestJsons.NoStagesJson);
         var questList = parseResult.Quests;
         var errors = parseResult.ErrorMessages;

         Assert.NotNull(questList);
         Assert.Empty(questList);
         Assert.Single(errors);
     }
     
     [Fact]
     public void LoadFromJson_CannotParseNoStageJsonList()
     {
         var parseResult = QuestParser.LoadFromJson(QuestJsons.QuestListNoStages);
         var questList = parseResult.Quests;
         var errors = parseResult.ErrorMessages;

         Assert.NotNull(questList);
         Assert.Single(questList);
         Assert.Single(errors);
     }

     [Fact]
     public void LoadFromJson_ShouldDeserializeSingleQuest()
     {
         var parseResult = QuestParser.LoadFromJson(QuestJsons.SmallQuestJson);
         Assert.NotNull(parseResult);
         
         var questList = parseResult.Quests;
         Assert.Single(questList);
        
         var quest = questList.First();
         Assert.NotNull(quest);
         Assert.Equal(1, quest.Id);
         Assert.Equal("First Quest", quest.Title);
         Assert.False(quest.IsCompleted);
        
         var stage = quest.CurrentStage;
         Assert.NotNull(stage);
         Assert.False(stage.IsCompleted);
         Assert.Equal("This is stage 1",stage.StageDescription);
         //Assert.Equal(0,stage.CompletedObjectiveCount);
     }
     
     [Fact]
     public void LoadFromJson_ShouldDeserializeMultipleQuests()
     {
         var parseResult = QuestParser.LoadFromJson(QuestJsons.MultiQuestJson);
         var questList = parseResult.Quests;
         Assert.NotNull(questList);
         Assert.Equal(2, questList.Count);
        
         var quest = questList.First();
         Assert.NotNull(quest);
         Assert.Equal(6, quest.Id);
         Assert.True(quest.IsMainQuest);
         Assert.Equal(7, quest.NextQuestId);
         Assert.Equal("First Quest In Multi", quest.Title);
         Assert.False(quest.IsCompleted);
         
         var quest2 = questList[1];
         Assert.NotNull(quest2);
         Assert.Equal(7, quest2.Id);
         Assert.Equal(0, quest2.NextQuestId);
         Assert.True(quest.IsMainQuest);
     }
    
     
     [Fact]
     public void ParseExampleJson_ShouldReturnExpectedModel()
     {
         var baseDir = AppDomain.CurrentDomain.BaseDirectory;
         var filePath = Path.Combine(baseDir, "Parser", "Example.json");
         var parseResult = QuestParser.LoadFromJsonFile(filePath);
         var questListFromJson = parseResult.Quests;
         Assert.NotNull(questListFromJson);
         Assert.NotEmpty(questListFromJson);
         Assert.Equal(2,questListFromJson.Count);
     }

}