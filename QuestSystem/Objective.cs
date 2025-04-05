namespace QuestSystem;

public class Objective
{
    private readonly int _taskTypeId;
    private readonly int _targetAssetId;
    private readonly int _goalValue;
    private int _currValue;
    
    // Simple Getters
    public int TaskTypeId => _taskTypeId;
    public int TargetAssetId => _targetAssetId;
    public int CurrValue => _currValue;
    public int GoalValue => _goalValue;
    
    // Complex Getters
    public string ProgressPrint => $"{_currValue}/{_goalValue}";
    public bool IsCompleted => _currValue >= _goalValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="Objective"/> class with the specified value and type.
    /// </summary>
    /// <param name="value">The required value to complete the objective.</param>
    /// <param name="taskTypeId">The id of the type of the task.</param>
    /// <param name="assetId">The specific asset that proceeds the objective</param>
    public Objective(int value, int taskTypeId, int assetId = -1)
    {
        _taskTypeId = taskTypeId;
        _goalValue = value;
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
    public void TryProceed(int progressValue, int assetId = -1)
    {
        // Completed objectives do not receive progress (positive or negative)
        if (IsCompleted) return;
        
        // When assetId is set, the objective can only proceed with the correct interaction
        if (_targetAssetId > 0 && _targetAssetId != assetId) return;

        var newCurrentValue = _currValue + progressValue;
        
        // In case of decrement it should not go below 0.
        if (newCurrentValue <= 0)
        {
            _currValue = 0;
            return;
        }
        
        // In case of increment it should cap at objective of the goal
        if (_currValue + progressValue >= _goalValue)
        {
            _currValue = _goalValue;
            return;
        }

        // Otherwise it will simply change to a new value
        _currValue += progressValue;
    }
}