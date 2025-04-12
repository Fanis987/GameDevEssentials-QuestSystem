using QuestSystem.Entities;

namespace QuestSystem.Parser.Dtos;

public record ObjectiveDto(int GoalValue,int TaskTypeId,int TargetAssetId) {
    
    public Objective ToObjective() {
        return new Objective(GoalValue, TaskTypeId, TargetAssetId);
    }
    
}