using QuestSystem.Entities;

namespace QuestSystem.Parser.Dtos;

internal record StagePathDto(bool IsSelective,int NextStageId, List<ObjectiveDto> Objectives)
{
    /// <summary>Produces a <see cref="StagePath"/> object from the dto.  </summary>
    /// <returns>The <see cref="StagePath"/> object</returns>
    internal StagePath ToStagePath() {
        var objectiveArray = Objectives.Select(objectiveDto => objectiveDto.ToObjective()).ToArray();
        return new StagePath(IsSelective,NextStageId,objectiveArray);
    }
}