namespace QuestSystem.Entities;

/// <summary>
/// Abstract class representing a completable stage of a quest.
/// </summary>
public abstract class QuestStage
{
    /// <summary>
    /// The objectives of the quest stage.
    /// </summary>
    protected readonly List<Objective> Objectives = new();
    
    //Getter properties
    public string StageDescription { get; protected set; }
    public bool IsCompleted { get; protected set; }
    
    // Complex Getters
    public IReadOnlyList<string> ObjectiveProgress => GetProgressOfStageObjectives();


    public abstract string StageProgress { get; }
    public int CompletedObjectiveCount => GetCompletedTaskCount();

    protected QuestStage(string stageDescription) {
        StageDescription = stageDescription;
    }
    
    /// <summary>
    /// Tries to progress tasks in the stage based on the provided progress data.
    /// </summary>
    /// <param name="objectiveProgressDto">The data containing task type and progress value.</param>
    public void TryProgressTask(ObjectiveProgressDto objectiveProgressDto)
    {
        //Try Advance some of the objectives
        foreach (var objective in Objectives)
        {
            if(objective.TaskTypeId != objectiveProgressDto.TaskTypeId) continue;
            
            objective.TryProceed(objectiveProgressDto.ProgressValue,objectiveProgressDto.AssetId);
        }

        CheckStageCompletion();
    }

    protected abstract void CheckStageCompletion();
  
    /// <summary>
    /// Returns number of completed tasks in the stage
    /// </summary>
    /// <returns>Number of completed tasks</returns>
    private int GetCompletedTaskCount()
    {
        var count = 0;
        foreach (var task in Objectives)
        {
            if (task.IsCompleted) count++;
        }
        return count;
    }
    
    /// <summary>
    /// Gets the progress for each of the objectives of this stage
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private List<string> GetProgressOfStageObjectives() {
        return Objectives.Select(objective => objective.ProgressPrint).ToList();
    }
}
