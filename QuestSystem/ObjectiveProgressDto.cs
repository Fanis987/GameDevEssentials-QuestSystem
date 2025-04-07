namespace QuestSystem;

/// <summary>
/// A data transfer object corresponding to progress in a specific task.
/// </summary>
public readonly struct ObjectiveProgressDto
{
    public int TaskTypeId { get; }
    public int ProgressValue { get; }
    public int AssetId { get;}
    
    /// <summary>
    /// Represents a progress unit for a quest objective
    /// </summary>
    /// <param name="taskTypeId">The id of the task progressed</param>
    /// <param name="progressValue">The amount of progress made</param>
    /// <param name="assetId">The id of the asset related to the progress</param>
    public ObjectiveProgressDto(int taskTypeId, int progressValue, int assetId = -1)
    {
        TaskTypeId = taskTypeId;
        ProgressValue = progressValue;
        AssetId = assetId;
    }

    public override string ToString() {
        return $"Objective Progress Dto- Progress: {ProgressValue} on taskId:{TaskTypeId} for asset:{AssetId}";
    }
}