using QuestSystem.Entities;

namespace QuestSystem.Parser.Dtos;

/// <summary>
/// Represents the quest stage info extracted from a json
/// </summary>
/// <param name="Description">The description of the stage</param>
/// <param name="IsCompleted">Whether the stage is completed</param>
/// <param name="IsSelective">Whether the stage is compelted by selecting an objective</param>
/// <param name="Objectives">The objectives of a stage</param>
internal record QuestStageDto(string Description,bool IsCompleted,bool IsSelective,List<ObjectiveDto> Objectives)
{
    /// <summary>
    /// Produces a QuestStage object from the dto.
    /// </summary>
    /// <returns>The QuestStage object</returns>
    internal QuestStage ToQuestStage() {
        var objectiveList = Objectives.Select(objectiveDto => objectiveDto.ToObjective()).ToList();
        return new QuestStage(Description,IsSelective, objectiveList);
    }

}