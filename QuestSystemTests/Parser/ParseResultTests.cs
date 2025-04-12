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
}