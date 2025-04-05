namespace QuestSystem;

/// <summary>
/// Represents a quest with one or more <see cref="QuestStage"/>.
/// </summary>
public class Quest
{
    // Main Properties
    public string Title {get; private set;}
    public int Id {get; private set;}
    
    private readonly Queue<QuestStage> _stagesQueue =  new();
    
    // Getter Properties
    public bool IsCompleted => _stagesQueue.Count == 0;
    public QuestStage? CurrentStage => IsCompleted? null : _stagesQueue.Peek();
    public int StagesLeft => _stagesQueue.Count;
    

    public Quest(int id, string title, params QuestStage[] stages)
    {
        //Arg checks
        if (id <= 0) {
            throw new ArgumentException("Invalid quest id", nameof(id));
        }
        if (string.IsNullOrEmpty(title)) {
            throw new ArgumentException("Title cannot be empty", nameof(title));
        }
        Id = id;
        Title = title;
        
        //Null checks
        ArgumentNullException.ThrowIfNull(stages, nameof(stages));
        foreach (var stage in stages)
        {
            ArgumentNullException.ThrowIfNull(stage, $"{nameof(stages)} must not be null");
        }
        
        // Fill queue
        foreach (var stage in stages)
        {
            _stagesQueue.Enqueue (stage);    
        }
    }

    /// <summary>
    /// Constructs a quest with one inclusive stage using the provided objectives
    /// </summary>
    /// <param name="id"> The id of the quest</param>
    /// <param name="title"> The title of the quest</param>
    /// <param name="isInclusiveStage"> True if the stage is inclusive, false if selective</param>
    /// <param name="stageDescription">The description of the quest stage</param>
    /// <param name="objectives"> The objectives for the sole stage</param>
    /// <exception cref="ArgumentException"></exception>
    public Quest(int id, string title,bool isInclusiveStage,
        string stageDescription, params Objective[] objectives)
    {
        //Arg checks
        if (id <= 0) {
            throw new ArgumentException("Invalid quest id", nameof(id));
        }
        if (string.IsNullOrEmpty(title)) {
            throw new ArgumentException("Title cannot be empty", nameof(title));
        }
        Id = id;
        Title = title;
        
        //Null checks
        ArgumentNullException.ThrowIfNull(objectives, nameof(objectives));
        foreach (var objective in objectives)
        {
            ArgumentNullException.ThrowIfNull(objective, 
                $"{nameof(objective)} must not be null");
        }
        
        // Create a quest stage and add to queue
        QuestStage stage;
        if (isInclusiveStage)
        {
            stage = new QuestStageInclusive(stageDescription,objectives);
            _stagesQueue.Enqueue(stage);
            return;
        }
        stage = new QuestStageSelective(stageDescription,objectives);
        _stagesQueue.Enqueue(stage);
    }

    /// <summary>
    /// Attempts to progress the quest based on the given information.
    /// </summary>
    /// <param name="objProgressDto"> The DTO containing the action that just happened</param>
    /// <exception cref="InvalidOperationException"></exception>
    public void TryProgressQuest(ObjectiveProgressDto objProgressDto)
    {
        // A completed quest cannot be progressed
        if(IsCompleted) return;
        
        //Try progress the current stage based on arg
        var currentStage = _stagesQueue.Peek();
        if (currentStage.IsCompleted) {
            throw new InvalidOperationException("The current stage is already completed");
        }
        currentStage.TryProgressTask(objProgressDto);

        // Stage not completed yet
        if (!currentStage.IsCompleted) return;
        
        // Stage just completed
        _stagesQueue.Dequeue(); //move to the next stage or finish
    }
}