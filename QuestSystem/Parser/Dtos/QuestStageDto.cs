using QuestSystem.Entities;

namespace QuestSystem.Parser.Dtos;

/// <summary>
/// Represents the quest stage info extracted from a json
/// </summary>
/// <param name="Description">The description of the stage</param>
/// <param name="IsCompleted">Whether the stage is completed</param>
/// <param name="Objectives">The objectives of a stage</param>
internal record QuestStageDto(int Id,string Description,bool IsCompleted, List<StagePathDto> PathDtos)
{
    /// <summary>
    /// Produces a QuestStage object from the dto.
    /// </summary>
    /// <returns>The QuestStage object</returns>
    internal QuestStage ToQuestStage() {
        var pathsArray = PathDtos.Select(pathDto => pathDto.ToStagePath()).ToArray();
        return new QuestStage(Id, Description, pathsArray);
    }

}