using QuestSystem.Entities;

namespace QuestSystem;

/// <summary>
/// A quest stage that can be completed by completing any of the objectives.
/// </summary>
public class QuestStageSelective: QuestStage
{

    public override string StageProgress => $"{CompletedObjectiveCount}/1";


    /// <summary>
    /// Initializes a new instance of the <see cref="QuestStage"/> class with a set of tasks.
    /// </summary>
    /// <param name="stageDescription"> The description of the stage</param>
    /// <param name="objectives">An array of tasks that belong to this quest stage.</param>
    /// <exception cref="ArgumentNullException">Thrown when null tasks are provided.</exception>
    /// <exception cref="ArgumentException">Thrown when no tasks are provided.</exception>
    public QuestStageSelective(string stageDescription, params Objective[] objectives):
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
    
    public QuestStageSelective(string stageDescription, List<Objective> objectives):
        this(stageDescription, objectives.ToArray()?? throw new ArgumentNullException()){}
    
    /// <summary>
    /// Checks any objective in the stage is completed and marks the stage as completed if they are.
    /// </summary>
    protected override void CheckStageCompletion()
    {
        if (Objectives.Any(task => task.IsCompleted)) IsCompleted = true;
    }
}