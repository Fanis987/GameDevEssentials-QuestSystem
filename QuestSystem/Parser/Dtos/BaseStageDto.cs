using QuestSystem.Entities;

namespace QuestSystem.Parser.Dtos;

internal record BaseStageDto(string Description,bool IsCompleted,bool IsSelective,List<ObjectiveDto> Objectives)
{
    internal QuestStage ToQuestStage()
    {
        var objectiveList = Objectives.Select(objectiveDto => objectiveDto.ToObjective()).ToList();

        if(IsSelective) {
            return new QuestStageSelective(Description, objectiveList);
        }

        return new QuestStageInclusive(Description, objectiveList);
    }

}