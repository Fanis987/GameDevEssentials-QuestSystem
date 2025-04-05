namespace QuestSystem;

/// <summary>
/// A data transfer object corresponding to progress in a specific task.
/// </summary>
public struct ObjectiveProgressDto
{
    public int TaskTypeId { get; init; }
    public int ProgressValue { get; init; }
    
    public int AssetId { get; init; }
    
    public ObjectiveProgressDto(int taskTypeId, int progressValue, int assetId = -1)
    {
        TaskTypeId = taskTypeId;
        ProgressValue = progressValue;
        AssetId = assetId;
    }
}