namespace QuestSystem.Entities;

public class StagePath
{
    /// <summary>The objectives of the stage path.</summary>
    private readonly List<Objective> _objectives = new();
    
    /// <summary> Whether this <see cref="StagePath"/> is complete</summary>
    public bool IsCompleted { get; private set; }
    
    /// <summary> Whether this <see cref="StagePath"/> can completed, by finishing ANY of its objectives</summary>
    public bool IsSelective { get;}
    
    /// <summary> The number of objectives completed in this <see cref="QuestStage"/></summary>
    public int CompletedObjectiveCount => _objectives.Count(objective => objective.IsCompleted);
    
    /// <summary> A simple progress indicator of the stage's objectives</summary>
    public string PathProgress => $"{CompletedObjectiveCount}/{_objectives.Count}";
    
    /// <summary> The list of individual progress of each <see cref="Objective"/> in this stage</summary>
    public IReadOnlyList<string> ObjectiveProgress => GetProgressOfStagePathObjectives();
    
    public StagePath(bool isSelective, params Objective[] objectives) {
        //Basic checks
        if (objectives == null) throw new ArgumentNullException(nameof(objectives),"Objectives cannot be null.");
        if (objectives.Length == 0) throw new ArgumentException("No objectives were passed");
        
        IsSelective = isSelective;
        for (var i = 0; i < objectives.Length; i++) {
            if (objectives[i] == null) {
                throw new ArgumentNullException(nameof(objectives),"A null objective found in the passed array");
            }
            _objectives.Add(objectives[i]);
        }
    }
    
    /// <summary>
    /// Tries to progress tasks in the stage based on the provided progress data.
    /// </summary>
    /// <param name="progressValue">The value of the progress made.</param>
    /// <param name="taskTypeId">The id of the type of the action.</param>
    /// <param name="assetId">The id of the asset that was affected by the action</param>
    /// <exception cref="InvalidOperationException"></exception>
    public void TryProgressPath(int progressValue, int taskTypeId, int assetId = 0) {
        //Try Advance some of the objectives
        foreach (var objective in _objectives) {
            if(objective.TaskTypeId != taskTypeId) continue;
            objective.TryProceed(progressValue,assetId);
        }
        CheckStagePathCompletion();
    }
    
    /// <summary>
    /// Checks any objective in the stage is completed and marks the stage as completed if they are.
    /// </summary>
    private void CheckStagePathCompletion() {
        if (IsSelective) { //At least one objective should be completed
            if (_objectives.Any(task => task.IsCompleted)) IsCompleted = true;
            return;
        }
        // ALl objectives must be completed
        if (_objectives.Any(task => ! task.IsCompleted)) return;
        IsCompleted = true;
    }
    
    /// <summary>
    /// Gets the progress for each of the objectives of this stage
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private List<string> GetProgressOfStagePathObjectives() {
        return _objectives.Select(objective => objective.ProgressPrint).ToList();
    }
    
}