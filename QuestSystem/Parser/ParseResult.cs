namespace QuestSystem.Parser;

public class ParseResult
{
    public bool IsSuccessful { get; set; }
    public string ErrorMessage { get; set; } = "";

    public static ParseResult Ok() => new() { IsSuccessful = true };

    public static ParseResult Fail(string errorMessage) => new() { IsSuccessful = false, ErrorMessage = errorMessage };
}