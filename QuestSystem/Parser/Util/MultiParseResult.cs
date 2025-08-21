using QuestSystem.Entities;

namespace QuestSystem.Parser.Util;

public class MultiParseResult
{
    public List<Quest> Quests { get; } = new ();
    public List<string> ErrorMessages { get; } = new ();
}