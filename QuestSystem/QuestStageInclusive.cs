namespace QuestSystem;

/// <summary>
/// A quest stage that requires all its objectives to be completed.
/// </summary>
public class QuestStageInclusive: QuestStage
{
    // Properties
    public int TotalObjectiveCount => Objectives.Count;
    public string StageProgress => $"{GetCompletedObjectiveCount()}/{TotalObjectiveCount}";

    /// <summary>
    /// Initializes a new instance of the <see cref="QuestStage"/> class with a set of tasks.
    /// </summary>
    /// <param name="stageDescription"> The description of this stage</param>
    /// <param name="objectives">An array of tasks that belong to this quest stage.</param>
    /// <exception cref="ArgumentNullException">Thrown when null tasks are provided.</exception>
    /// <exception cref="ArgumentException">Thrown when no tasks are provided.</exception>
    public QuestStageInclusive(string stageDescription, params Objective[] objectives):
        base(stageDescription)
    {
        //Basic checks
        ArgumentNullException.ThrowIfNull(objectives);
        if (objectives.Length == 0) throw new Exception("No tasks are available");
        
        //Add to task list
        for (var i = 0; i < objectives.Length; i++)
        {
            if (objectives[i]==null) throw new ArgumentNullException();
            Objectives.Add(objectives[i]);
        }
    }
    
    /// <summary>
    /// Checks if all tasks in the stage are completed and marks the stage as completed if they are.
    /// </summary>
    protected override void CheckStageCompletion()
    {
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