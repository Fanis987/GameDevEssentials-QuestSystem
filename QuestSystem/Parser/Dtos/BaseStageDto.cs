namespace QuestSystem.Parser.Dtos;

public class BaseStageDto
{

    public string Description { get; set; } = "";
    public bool IsCompleted { get; set; } 
    public bool IsSelective { get; set; }
    public List<ObjectiveDto> Objectives { get; set; } = new();

}