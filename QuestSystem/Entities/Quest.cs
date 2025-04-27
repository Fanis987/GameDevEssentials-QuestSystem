namespace QuestSystem.Entities;

/// <summary>
/// Represents a quest with one or more <see cref="QuestStage"/>.
/// </summary>
public class Quest
{
    // Main Properties (set in ctor)
    private readonly Queue<QuestStage> _stagesQueue =  new();
    
    /// <summary> The id of the quest. Must be positive </summary>
    public int Id {get;}
    /// <summary> The title of the quest.</summary>
    public string Title {get;}
    /// <summary>Whether the quest is part of the main line.</summary>
    public bool IsMainQuest {get;}

    /// <summary>Whether the quest is part of the main line.</summary>
    public int NextQuestId { get; }
    /// <summary> Whether this <see cref="Quest"/> was failed</summary>
    public bool WasFailed { get; private set; }
    
    // Getter Properties
    /// <summary> Whether this <see cref="Quest"/> is complete </summary>
    public bool IsCompleted => _stagesQueue.Count == 0;
    /// <summary> The current <see cref="QuestStage"/> of the quest </summary>
    public QuestStage? CurrentStage => IsCompleted? null : _stagesQueue.Peek();
    /// <summary> The number of <see cref="QuestStage"/> left in this quest </summary>
    public int StagesLeft => _stagesQueue.Count;
    
    /// <summary>
    /// Represents a quest in a game.
    /// </summary>
    /// <param name="questId">The id of the quest</param>
    /// <param name="questTitle">The title of the quest</param>
    /// <param name="stages"> The stages of the quest, in order.</param>
    /// <exception cref="ArgumentException"></exception>
    public Quest(int questId, string questTitle, params QuestStage[] stages)
    {
        //Arg checks
        if (questId <= 0) {
            throw new ArgumentException("Invalid quest id", nameof(questId));
        }
        if (string.IsNullOrEmpty(questTitle)) {
            throw new ArgumentException("Title cannot be empty", nameof(questTitle));
        }
        Id = questId;
        Title = questTitle;
        
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
    /// Represents a quest in a game.
    /// </summary>
    /// <param name="questId">The id of the quest</param>
    /// <param name="questTitle">The title of the quest</param>
    /// <param name="isMainQuest">Whether the quest is part of the main quest line</param>
    /// <param name="stages"> The stages of the quest, in order.</param>
    public Quest(int questId, string questTitle,bool isMainQuest, params QuestStage[] stages)
    :this(questId,questTitle,stages)
    {
        IsMainQuest = isMainQuest;
    }

    /// <summary>
    /// Represents a quest in a game.
    /// </summary>
    /// <param name="questId">The id of the quest</param>
    /// <param name="questTitle">The title of the quest</param>
    /// <param name="isMainQuest">Whether the quest is part of the main quest line</param>
    /// <param name="nextQuestId">The id of the next quest in the quest chain. Zero interpreted as no next quest.</param>
    /// <param name="stages"> The stages of the quest, in order.</param>
    public Quest(int questId, string questTitle,bool isMainQuest,int nextQuestId, params QuestStage[] stages)
        :this(questId,questTitle,isMainQuest,stages)
    {
        NextQuestId = nextQuestId;
    }

    public Quest(int questId, string questTitle, List<QuestStage> stages)
        :this(questId, questTitle, stages.ToArray() ?? throw new NullReferenceException(nameof(stages))) { }

    /// <summary>
    /// Constructs a quest with one inclusive stage using the provided objectives
    /// </summary>
    /// <param name="questId"> The id of the quest</param>
    /// <param name="questTitle"> The title of the quest</param>
    /// <param name="isSelectiveStage">Whether stages completes by finishing ANY of the objectives</param>
    /// <param name="stageDescription">The description of the quest stage</param>
    /// <param name="objectives"> The objectives for the sole stage</param>
    /// <exception cref="ArgumentException"></exception>
    public Quest(int questId, string questTitle,bool isSelectiveStage,
        string stageDescription, params Objective[] objectives)
    {
        //Arg checks
        if (questId <= 0) {
            throw new ArgumentException("Invalid quest id", nameof(questId));
        }
        if (string.IsNullOrEmpty(questTitle)) {
            throw new ArgumentException("Title cannot be empty", nameof(questTitle));
        }
        Id = questId;
        Title = questTitle;
        
        //Null checks
        ArgumentNullException.ThrowIfNull(objectives, nameof(objectives));
        foreach (var objective in objectives)
        {
            ArgumentNullException.ThrowIfNull(objective, 
                $"{nameof(objective)} must not be null");
        }
        
         //Create a quest stage and add to queue
         var stage = new QuestStage(stageDescription,isSelectiveStage,objectives);
         _stagesQueue.Enqueue(stage);
    }

    /// <summary>
    /// Attempts to progress the quest based on the given information.
    /// </summary>
    /// <param name="objProgressDto"> The DTO containing the action that just happened</param>
    /// <exception cref="InvalidOperationException"></exception>
    public void TryProgressQuest(ObjectiveProgressDto objProgressDto)
    {
        // A completed or failed quest cannot be progressed
        if(IsCompleted) return;
        if(WasFailed) return;
        
        //Try progress the current stage based on arg
        var currentStage = _stagesQueue.Peek();
        if (currentStage.IsCompleted) {
            throw new InvalidOperationException("The current stage is already completed");
        }
        currentStage.TryProgressObjective(objProgressDto);

        // Stage not completed yet
        if (!currentStage.IsCompleted) return;
        
        // Stage just completed
        _stagesQueue.Dequeue(); //move to the next stage or finish
    }

    /// <summary>
    /// Attempts to progress the quest.
    /// </summary>
    /// <param name="progressValue">The value of the progress made.</param>
    /// <param name="taskTypeId">The id of the type of the action.</param>
    /// <param name="assetId">The Id of the asset that was affected by the action</param>
    /// <exception cref="InvalidOperationException"></exception>
    public void TryProgressQuest(int progressValue, int taskTypeId, int assetId)
    {
        var dto = new ObjectiveProgressDto(taskTypeId,progressValue, assetId);
        TryProgressQuest(dto);
    }

    /// <summary>
    /// Force skips the current stage of the quest.
    /// Useful when debugging and loading a half-finished quest from a save file.
    /// </summary>
    public void TrySkipStage() {
        if(IsCompleted) return; // A completed quest cannot be progressed
        _stagesQueue.Dequeue();  // Force-Skip the current stage
    }
    
    /// <summary>  Forces a quest to complete instantly. Useful for debugging.  </summary>
    public void CompleteInstantly() {
        _stagesQueue.Clear(); // Force-Skip all teh stages
    }

    /// <summary>Fails a quest. Useful for cases like timed quest </summary>
    public void Fail() => WasFailed = true;
}