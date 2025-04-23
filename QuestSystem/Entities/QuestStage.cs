namespace QuestSystem.Entities;

/// <summary>
/// Abstract class representing a completable stage of a quest.
/// </summary>
public class QuestStage
{
    /// <summary>
    /// The objectives of the quest stage.
    /// </summary>
    protected readonly List<Objective> Objectives = new();
    
    //Getter properties
    public string StageDescription { get; private set; }
    public bool IsCompleted { get; private set; }
    public bool IsSelective { get; private set; }
    
    // Complex Getters
    public IReadOnlyList<string> ObjectiveProgress => GetProgressOfStageObjectives();
    
    public string StageProgress => $"{GetCompletedObjectiveCount()}/{Objectives.Count}";
    public int CompletedObjectiveCount => GetCompletedTaskCount();

    public QuestStage(string stageDescription, params Objective[] objectives) {
        StageDescription = stageDescription;
        //Basic checks
        ArgumentNullException.ThrowIfNull(objectives);
        if (objectives.Length == 0)
        {
            throw new ArgumentException("No tasks are available");
        }
        
        //Add to task list
        for (var i = 0; i < objectives.Length; i++)
        {
            if (objectives[i] == null)
            {
                throw new ArgumentNullException();
            }
            Objectives.Add(objectives[i]);
        }
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
    
    /// <summary>
    /// Checks any objective in the stage is completed and marks the stage as completed if they are.
    /// </summary>
    private void CheckStageCompletion()
    {
        if (Objectives.Any(task => task.IsCompleted)) IsCompleted = true;
        if (Objectives.Any(task => ! task.IsCompleted)) return;
        IsCompleted = true;
    }
    

    /// <summary>
    /// Calculates the number of completed objectives in this stage
    /// </summary>
    /// <returns>Number of completed objectives</returns>
    private int GetCompletedObjectiveCount() {
        return Objectives.Count(objective => objective.IsCompleted);
    }
}
