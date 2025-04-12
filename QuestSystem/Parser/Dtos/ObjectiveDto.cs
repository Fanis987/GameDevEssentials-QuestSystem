using QuestSystem.Entities;

namespace QuestSystem.Parser.Dtos;

internal record ObjectiveDto(int GoalValue,int TaskTypeId,int TargetAssetId) {
    
    internal Objective ToObjective() {
        return new Objective(GoalValue, TaskTypeId, TargetAssetId);
    }
    
}