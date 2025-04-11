namespace QuestSystem.Parser.Dtos;

public class QuestDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public List<BaseStageDto> Stages { get; set; }
}