using QuestSystem.Entities;

namespace QuestSystem.Parser.Dtos;

/// <summary>
/// Represents the quest info extracted from a json
/// </summary>
/// <param name="Id">The id of the quest</param>
/// <param name="Title">The title of the quest</param>
/// <param name="Stages">The list of the stages</param>
internal record QuestDto(int Id,string Title,List<BaseStageDto> Stages)
{
    /// <summary>
    /// Produces a Quest object from the dto.
    /// </summary>
    /// <returns>The quest object with all the dto info</returns>
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