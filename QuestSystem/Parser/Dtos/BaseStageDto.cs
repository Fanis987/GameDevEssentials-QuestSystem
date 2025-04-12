using QuestSystem.Entities;

namespace QuestSystem.Parser.Dtos;

public record BaseStageDto(string Description,bool IsCompleted,bool IsSelective,List<ObjectiveDto> Objectives)
{
    public QuestStage ToQuestStage()
    {
        var objectiveList = Objectives.Select(objectiveDto => objectiveDto.ToObjective()).ToList();

        if(IsSelective) {
            return new QuestStageSelective(Description, objectiveList);
        }

        return new QuestStageInclusive(Description, objectiveList);
    }

}