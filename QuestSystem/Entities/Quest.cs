namespace QuestSystem.Entities;

/// <summary>
/// Represents a quest with one or more <see cref="QuestStage"/>.
/// </summary>
public class Quest
{
    // Main Properties (set in ctor)
    private readonly List<QuestStage> _allStages =  new();
    
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
    public bool IsCompleted { get; private set; } 
    
    /// <summary> The id of the quest. Must be positive </summary>
    public int CurrentStageId {get; private set; }
    /// <summary> The current <see cref="QuestStage"/> of the quest </summary>
    public QuestStage? CurrentStage => _allStages.FirstOrDefault(stage => stage.Id == CurrentStageId);

    
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
        ArgumentNullException.ThrowIfNull(stages, nameof(stages));
        foreach (var stage in stages) {
            ArgumentNullException.ThrowIfNull(stage, $"{nameof(stages)} must not be null");
        }
        if(HasDuplicateStageId(stages.ToList())) throw new QuestException("Duplicate stage ids found", nameof(stages));
        
        // Setting properties
        Id = questId;
        Title = questTitle;
        
        // Fill queue
        foreach (var stage in stages) {
            _allStages.Add(stage);
        }
        CurrentStageId = stages[0].Id;
    }

    /// <summary>
    /// Represents a quest in a game.
    /// </summary>
    /// <param name="questId">The id of the quest</param>
    /// <param name="questTitle">The title of the quest</param>
    /// <param name="isMainQuest">Whether the quest is part of the main quest line</param>
    /// <param name="stages"> The stages of the quest, in order.</param>
    public Quest(int questId, string questTitle,bool isMainQuest, params QuestStage[] stages)
    :this(questId,questTitle,stages) {
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
        :this(questId,questTitle,isMainQuest,stages) {
        NextQuestId = nextQuestId;
    }

    public Quest(int questId, string questTitle, List<QuestStage> stages)
        :this(questId, questTitle, stages.ToArray() ?? throw new NullReferenceException(nameof(stages))) { }

    /// <summary>
    /// Constructs a quest with one inclusive stage using the provided objectives
    /// </summary>
    /// <param name="questId"> The id of the quest</param>
    /// <param name="questTitle"> The title of the quest</param>
    /// <param name="isSelectiveStagePath">Whether stage with a singe stagePath completes by finishing ANY of the objectives</param>
    /// <param name="stageDescription">The description of the quest stage</param>
    /// <param name="objectives"> The objectives for the sole stage</param>
    /// <exception cref="ArgumentException"></exception>
    public Quest(int questId, string questTitle, bool isSelectiveStagePath,
        string stageDescription, params Objective[] objectives)
    {
        // Argument checks
        if (questId <= 0) {
            throw new ArgumentException("Invalid quest id", nameof(questId));
        }
        if (string.IsNullOrEmpty(questTitle)) {
            throw new ArgumentException("Title cannot be empty", nameof(questTitle));
        }
        ArgumentNullException.ThrowIfNull(objectives, nameof(objectives));
        foreach (var objective in objectives)
        {
            ArgumentNullException.ThrowIfNull(objective,
                $"{nameof(objective)} must not be null");
        }

        Id = questId;
        Title = questTitle;

        // Create a stage path and a stage from it
        var stagePath = new StagePath(isSelectiveStagePath,-1, objectives.ToArray());
        var questStage = new QuestStage(1, stageDescription, stagePath);
        _allStages.Add(questStage);
    }
    
    
    /// <summary>
    /// Helper method that detects stages with duplicate ids
    /// </summary>
    /// <param name="stages">The list of stages passed by the user.</param>
    /// <returns>Whether a duplicate stage id was found.</returns>
    private bool HasDuplicateStageId(List<QuestStage> stages) {
        if(stages.Count == 1) return false;
        for (int i = 0; i < stages.Count; i++) {
            for (int j = 0; j < i; j++) {
                if(stages[i].Id == stages[j].Id) return true;
            }
        }
        return false;
    }

    #region Quest Progressing
    
    public void TryProgressQuest(int progressValue, int taskTypeId, int assetId = -1)
    {
        // A completed or failed quest cannot be progressed
        if(IsCompleted) return;
        if(WasFailed) return;
        
        //Try progress the current stage based on arg
        //var currentStage = _stagesQueue.Peek();
        var currentStage = _allStages.FirstOrDefault(stage => stage.Id == CurrentStageId);
        if(currentStage == null) throw new InvalidOperationException("Cannot identify current stage");
        if (currentStage.IsCompleted) {
            throw new InvalidOperationException("The current stage is already completed");
        }
        currentStage.TryProgressStage(progressValue, taskTypeId,  assetId);

        // Stage not completed yet - move to the next stage or finish
        if (!currentStage.IsCompleted) return;
        if (currentStage.NextStageId == -1) {//quest done
            IsCompleted = true;
            return;
        }
        CurrentStageId = currentStage.NextStageId;
    }
    
    /// <summary>  Forces a quest to complete instantly. Useful for debugging.  </summary>
    public void CompleteInstantly() {
        IsCompleted = true;
    }

    /// <summary>Fails a quest. Useful for cases like timed quest </summary>
    public void Fail() => WasFailed = true;
    
    #endregion
}