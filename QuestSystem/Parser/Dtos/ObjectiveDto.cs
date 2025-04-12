using QuestSystem.Entities;

namespace QuestSystem.Parser.Dtos;

/// <summary>
/// Represents the quest stage info extracted from a json
/// </summary>
/// <param name="GoalValue">The goal value of the objective</param>
/// <param name="TaskTypeId">The id of the task for the objective</param>
/// <param name="TargetAssetId">The asset that can progress the objective</param>
internal record ObjectiveDto(int GoalValue,int TaskTypeId,int TargetAssetId) {
    
    /// <summary>
    /// Produces an Objective object from the dto.
    /// </summary>
    /// <returns></returns>
    internal Objective ToObjective() {
        return new Objective(GoalValue, TaskTypeId, TargetAssetId);
    }
    
}