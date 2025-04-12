using QuestSystem.Parser;

namespace QuestSystemTests.Parser;

public class ParseResultTests
{
    [Fact]
    public void Ok_ShouldReturnSuccessfulResult()
    {
        // Act
        var result = ParseResult.Ok();

        // Assert
        Assert.True(result.IsSuccessful);
        Assert.Equal(string.Empty, result.ErrorMessage);
    }

    [Fact]
    public void Fail_ShouldReturnFailedResultWithErrorMessage()
    {
        // Arrange
        var error = "Parsing failed due to invalid format.";

        // Act
        var result = ParseResult.Fail(error);

        // Assert
        Assert.False(result.IsSuccessful);
        Assert.Equal(error, result.ErrorMessage);
    }

    [Fact]
    public void Fail_ShouldDefaultToNotSuccessful()
    {
        // Act
        var result = ParseResult.Fail("Some error");

        // Assert
        Assert.False(result.IsSuccessful);
    }

    [Fact]
    public void LoadFromJson_ShouldDeserializeSingleQuest()
    {
        var questList = QuestParser.LoadFromJson(QuestJsons.SmallQuestJson);
        
        Assert.NotNull(questList);
        Assert.Single(questList);
        
        var quest = questList.First();
        Assert.NotNull(quest);
        Assert.Equal(1, quest.Id);
        Assert.Equal("First Quest", quest.Title);
        Assert.False(quest.IsCompleted);
        Assert.Equal(1,quest.StagesLeft);
        
        var stage = quest.CurrentStage;
        Assert.NotNull(stage);
        Assert.False(stage.IsCompleted);
        Assert.Equal("This is stage 1",stage.StageDescription);
        Assert.Equal(0,stage.CompletedObjectiveCount);
        
    }
}