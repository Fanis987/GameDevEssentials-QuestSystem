namespace QuestSystem;

public class QuestException : ArgumentException
{
    public QuestException(string? message, string? paramName):base(message,paramName){}
}