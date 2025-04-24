using QuestSystem.Entities;

namespace QuestSystem.Parser;

public class MultiParseResult
{
    public List<Quest> Quests { get; } = new ();
    public List<string> ErrorMessages { get; } = new ();
}