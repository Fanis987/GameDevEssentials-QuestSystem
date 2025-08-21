namespace QuestSystem.Parser.Util;

/// <summary>
/// Represents the result of a parsing attempt of a quest json text
/// </summary>
public class ParseResult
{
    public bool IsSuccessful { get; set; }
    public string ErrorMessage { get; set; } = "";

    public static ParseResult Ok() => new() { IsSuccessful = true };

    public static ParseResult Fail(string errorMessage) => new() { IsSuccessful = false, ErrorMessage = errorMessage };
}