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
    /// <summary> The id of the <see cref="QuestStage"/>. Must be positive.</summary>
    public int Id { get; }
    /// <summary> The description of the <see cref="QuestStage"/> </summary>
    public string StageDescription { get; }
    /// <summary> Whether this <see cref="QuestStage"/> is complete</summary>
    public bool IsCompleted { get; private set; }
    /// <summary> Whether this <see cref="QuestStage"/> can completed, by finishing ANY of its objectives</summary>
    public bool IsSelective { get;}
    /// <summary>The time left for this stage. Zero means the stage is NOT timed.</summary>
    public float TimeLeft { get; private set; }
    
    
    // Complex Getters
    /// <summary> The list of individual progress of each <see cref="Objective"/> in this stage</summary>
    public IReadOnlyList<string> ObjectiveProgress => GetProgressOfStageObjectives();
    /// <summary> A simple progress indicator of the stage's objectives</summary>
    public string StageProgress => $"{GetCompletedObjectiveCount()}/{_objectives.Count}";
    /// <summary> The number of objectives completed in this <see cref="QuestStage"/></summary>
    public int CompletedObjectiveCount => GetCompletedTaskCount();

    /// <summary>
    /// Represents a stage of a quest.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="stageDescription">Description of the stage</param>
    /// <param name="isSelective">Whether the stage is completed by completing ANY of its objectives</param>
    /// <param name="objectives">The objectives of the stage</param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public QuestStage(int id, string stageDescription, bool isSelective, IList<Objective> objectives) {
        //Basic checks
        ArgumentNullException.ThrowIfNull(stageDescription);
        ArgumentNullException.ThrowIfNull(objectives);
        if (id <= 0) {
            throw new ArgumentOutOfRangeException(nameof(id),"Id must be positive");
        }
        if (objectives.Count() == 0) {
            throw new ArgumentException("No objectives were passed");
        }
        if (stageDescription.Length == 0) {
            throw new ArgumentException("Must have a stage description");
        }
        
        // Fill fields
        Id = id;
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
    public void TryProgressObjective(ObjectiveProgressDto objectiveProgressDto) {
        //Try Advance some of the objectives
        foreach (var objective in _objectives) {
            if(objective.TaskTypeId != objectiveProgressDto.TaskTypeId) continue;
            objective.TryProceed(objectiveProgressDto.ProgressValue,objectiveProgressDto.AssetId);
        }
        CheckStageCompletion();
    }
    
    /// <summary>
    /// Tries to progress tasks in the stage based on the provided progress data.
    /// </summary>
    /// <param name="progressValue">The value of the progress made.</param>
    /// <param name="taskTypeId">The id of the type of the action.</param>
    /// <param name="assetId">The id of the asset that was affected by the action</param>
    /// <exception cref="InvalidOperationException"></exception>
    public void TryProgressObjective(int progressValue, int taskTypeId, int assetId = 0) {
        var dto = new ObjectiveProgressDto(taskTypeId,progressValue, assetId);
        TryProgressObjective(dto);
    }
    
    /// <summary>
    /// Checks any objective in the stage is completed and marks the stage as completed if they are.
    /// </summary>
    private void CheckStageCompletion() {
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
    private int GetCompletedTaskCount() {
        var count = 0;
        foreach (var task in _objectives) {
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
    
    /// <summary>Makes the quest timed</summary>
    public void MakeTimed(float newTime) => TimeLeft = newTime;
}
