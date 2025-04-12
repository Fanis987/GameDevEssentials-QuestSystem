using QuestSystem.Entities;

namespace QuestSystem.Parser.Dtos;

internal record QuestDto(int Id,string Title,List<BaseStageDto> Stages)
{
    internal Quest ToQuest()
    {
        var stagesList = new List<QuestStage>();
        foreach (var stageDto in Stages)
        {
            stagesList.Add(stageDto.ToQuestStage());
        }
        return new Quest(Id, Title,stagesList);
    }
}