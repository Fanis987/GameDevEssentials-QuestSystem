using System.Text.Json;
using QuestSystem.Parser;
using QuestSystem.Parser.Dtos;

namespace QuestSystemTests.Parser;

public class QuestParserTests
{
    
    [Fact]
    public void CanParseSmallJson()
    {
        var questDto = JsonSerializer.Deserialize<QuestDto>(QuestJsons.SmallQuestJson);
        
        Assert.NotNull(questDto);
        Assert.Equal(1,questDto.Id);
        Assert.Equal("First Quest",questDto.Title);
    
        var stages = questDto.Stages;
        Assert.NotNull(stages);
        Assert.Single(stages);
        Assert.False(stages[0].IsCompleted);
        Assert.Equal("This is stage 1",stages[0].Description);
        
        var objectives = stages[0].Objectives;
        Assert.NotNull(objectives);
        Assert.Single(objectives);
        Assert.Equal(5,objectives[0].GoalValue);
        Assert.Equal(3,objectives[0].TaskTypeId);
        Assert.Equal(2,objectives[0].TargetAssetId);
    }
    
     [Fact]
     public void IsValidDto_ShouldReturnOkForValidQuestDto()
     {
         var questDto = JsonSerializer.Deserialize<QuestDto>(QuestJsons.SmallQuestJson);
         Assert.NotNull(questDto);
         var result = QuestParser.IsValidDto(questDto);

         Assert.True(result.IsSuccessful);
         Assert.Equal(string.Empty, result.ErrorMessage);
     }
        
     
     [Fact]
     public void IsValidDto_ShouldReturnOkForValidQuestDto2()
     {
         var questDto = JsonSerializer.Deserialize<QuestDto>(QuestJsons.MediumQuestJson);
         Assert.NotNull(questDto);
         var result = QuestParser.IsValidDto(questDto);

         Assert.True(result.IsSuccessful);
         Assert.Equal(string.Empty, result.ErrorMessage);
     }
     
     [Fact]
     public void IsValidDto_ShouldFailWhenNoStagesPresent()
     {
         var questDto = JsonSerializer.Deserialize<QuestDto>(QuestJsons.NoStagesJson);
         Assert.NotNull(questDto);
         var result = QuestParser.IsValidDto(questDto);

         Assert.False(result.IsSuccessful);
         Assert.Contains("No Stages Found", result.ErrorMessage);
     }
     
     [Fact]
     public void IsValidDto_ShouldFailWhenStageIsCompleted()
     {
         var questDto = JsonSerializer.Deserialize<QuestDto>(QuestJsons.CompletedStageJson);
         Assert.NotNull(questDto);
         var result = QuestParser.IsValidDto(questDto);

         Assert.False(result.IsSuccessful);
         Assert.Contains("Completed stage found", result.ErrorMessage);
     }

     [Fact]
     public void IsValidDto_ShouldFail_WhenStageHasNoObjectives()
     {
         var questDto = JsonSerializer.Deserialize<QuestDto>(QuestJsons.StageWithNoObjectivesJson);
         Assert.NotNull(questDto);
         var result = QuestParser.IsValidDto(questDto);

         Assert.False(result.IsSuccessful);
         Assert.Contains("No Objectives Found", result.ErrorMessage);
     }

}