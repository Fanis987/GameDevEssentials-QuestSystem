using ArgumentException = System.ArgumentException;

namespace QuestSystem.Entities;

/// <summary>
/// Abstract class representing a completable stage of a quest.
/// </summary>
public class QuestStage
{
    /// <summary>
    /// The objectives of the quest stage.
    /// </summary>
    private readonly List<Objective> _objectives = new();
    
    //Getter properties
    public string StageDescription { get; private set; }
    public bool IsCompleted { get; private set; }
    public bool IsSelective { get; private set; }
    
    // Complex Getters
    public IReadOnlyList<string> ObjectiveProgress => GetProgressOfStageObjectives();
    public string StageProgress => $"{GetCompletedObjectiveCount()}/{_objectives.Count}";
    public int CompletedObjectiveCount => GetCompletedTaskCount();

    /// <summary>
    /// Represents a stage of a quest.
    /// </summary>
    /// <param name="stageDescription">Description of the stage</param>
    /// <param name="isSelective">Whether the stage is completed by completing ANY of its objectives</param>
    /// <param name="objectives">The objectives of the stage</param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public QuestStage(string stageDescription, bool isSelective, IList<Objective> objectives) {
        //Basic checks
        ArgumentNullException.ThrowIfNull(stageDescription);
        ArgumentNullException.ThrowIfNull(objectives);
        if (objectives.Count() == 0) {
            throw new ArgumentException("No objectives were passed");
        }
        if (stageDescription.Length == 0) {
            throw new ArgumentException("Must have a stage description");
        }
        
        // Fill fields
        StageDescription = stageDescription;
        IsSelective = isSelective;
        for (var i = 0; i < objectives.Count; i++) {
            if (objectives[i] == null) {
                throw new ArgumentException("A null objective found in the passed list");
            }
            _objectives.Add(objectives[i]);
        }
    }
    
    /// <summary>
    /// Tries to progress tasks in the stage based on the provided progress data.
    /// </summary>
    /// <param name="objectiveProgressDto">The data containing task type and progress value.</param>
    public void TryProgressTask(ObjectiveProgressDto objectiveProgressDto)
    {
        //Try Advance some of the objectives
        foreach (var objective in _objectives)
        {
            if(objective.TaskTypeId != objectiveProgressDto.TaskTypeId) continue;
            
            objective.TryProceed(objectiveProgressDto.ProgressValue,objectiveProgressDto.AssetId);
        }
        CheckStageCompletion();
    }
    
    /// <summary>
    /// Checks any objective in the stage is completed and marks the stage as completed if they are.
    /// </summary>
    private void CheckStageCompletion()
    {
        if (IsSelective) { //At least one objective should be completed
            if (_objectives.Any(task => task.IsCompleted)) IsCompleted = true;
            return;
        }
        // ALl objectives must be completed
        if (_objectives.Any(task => ! task.IsCompleted)) return;
        IsCompleted = true;
    }
    
    /// <summary>
    /// Returns number of completed tasks in the stage
    /// </summary>
    /// <returns>Number of completed tasks</returns>
    private int GetCompletedTaskCount()
    {
        var count = 0;
        foreach (var task in _objectives)
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
        return _objectives.Select(objective => objective.ProgressPrint).ToList();
    }
    
    /// <summary>
    /// Calculates the number of completed objectives in this stage
    /// </summary>
    /// <returns>Number of completed objectives</returns>
    private int GetCompletedObjectiveCount() {
        return _objectives.Count(objective => objective.IsCompleted);
    }
}
