using QuestSystem.Entities;
using QuestSystem.Parser.Util;

namespace QuestSystemTests.Parser.Util;

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
    public void CanMakeMultipleParseResults() {
        var result = new MultiParseResult();
        
        Assert.NotNull(result);
        Assert.Empty(result.Quests);
        Assert.Empty(result.ErrorMessages);

        var path = new StagePath(true, 3, new Objective(5, 3));
        var stage = new QuestStage(3, "sth",path);
        var quest = new Quest(1, "title",stage );
        result.Quests.Add(quest);
        result.ErrorMessages.Add("error message");
        Assert.Single(result.Quests);
        Assert.Single(result.ErrorMessages);
    }
}