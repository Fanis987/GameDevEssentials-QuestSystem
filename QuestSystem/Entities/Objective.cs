namespace QuestSystem.Entities;

/// <summary>
/// Represents an objective of a quest stage.
/// </summary>
public class Objective
{
    private readonly int _taskTypeId;
    private readonly int _targetAssetId;
    private readonly int _goalValue;
    private int _currValue;
    
    // Simple Getters
    /// <summary> The current progress of the objective. When equal to <see cref="GoalValue"/> objective is complete </summary>
    public int CurrValue => _currValue;
    /// <summary> The goal progress of the objective </summary>
    public int GoalValue => _goalValue;
    /// <summary> The id of the action of this objective </summary>
    public int TaskTypeId => _taskTypeId;
    /// <summary> OPTIONAL: The id of the asset that must be interacted-with, for the objective to progress</summary>
    public int TargetAssetId => _targetAssetId;
    
    // Complex Getters
    /// <summary> A simple progress indicator</summary>
    public string ProgressPrint => $"{_currValue}/{_goalValue}";
    /// <summary> Whether this <see cref="Objective"/> is complete</summary>
    public bool IsCompleted => _currValue >= _goalValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="Objective"/> class with the specified value and type.
    /// Note: AssetId  = 0 is a wildcard, meaning ANY asset (e.g. orc, toad, dragon), affected by the task performed (e.g. killed) can progress this objective
    /// </summary>
    /// <param name="goalValue">The required value to complete the objective.</param>
    /// <param name="taskTypeId">The id of the type of the task.</param>
    /// <param name="assetId">The specific asset that proceeds the objective</param>
    public Objective(int goalValue, int taskTypeId, int assetId = 0) {
        // Arg Checks
        if(goalValue <= 0) throw new ArgumentException("Goal value must be greater than zero.");
        if(taskTypeId < 0) throw new ArgumentException("Task type id must not be negative.");
        if(assetId < 0) throw new ArgumentException("Asset id must not be negative.");
        //Init vals
        _taskTypeId = taskTypeId;
        _goalValue = goalValue;
        _targetAssetId = assetId;
        _currValue = 0;
    }

    /// <summary>
    /// Tries to proceed with the task by adding progress.
    /// Added progress can be positive or negative depending on the objective.
    /// The underlying <see cref="_currValue"/> will never go below 0.
    /// </summary>
    /// <param name="progressValue">The progress value to add.</param>
    /// <param name="assetId">The specific asset that proceeds the objective.</param>
    public void TryProceed(int progressValue, int assetId = 0)
    {
        // Completed objectives do not receive progress (positive or negative)
        if (IsCompleted) return;
        
        // When assetId is set, the objective can only proceed with the correct interaction
        if (_targetAssetId > 0 && _targetAssetId != assetId) return;

        var newCurrentValue = _currValue + progressValue;
        
        // In case of decrement it should not go below 0.
        if (newCurrentValue <= 0) {
            _currValue = 0;
            return;
        }
        
        // In case of increment it should cap at objective of the goal
        if (_currValue + progressValue >= _goalValue) {
            _currValue = _goalValue;
            return;
        }

        // Otherwise it will simply change to a new value
        _currValue += progressValue;
    }
}