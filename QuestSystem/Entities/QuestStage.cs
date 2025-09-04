using ArgumentException = System.ArgumentException;

namespace QuestSystem.Entities;

/// <summary>Represents a completable stage of a quest.</summary>
public class QuestStage
{
    /// <summary> The objectives of the quest stage. </summary>
    private readonly List<StagePath> _paths = new();
    /// <summary>The id of the next stage, will be decided when one of the stage paths is completed </summary>
    private int _nextStageId;
    
    //Getter properties
    /// <summary> The id of the <see cref="QuestStage"/>. Must be positive.</summary>
    public int Id { get; }
    /// <summary> The description of the <see cref="QuestStage"/> </summary>
    public string Description { get; }
    /// <summary> Whether this <see cref="QuestStage"/> is complete</summary>
    public bool IsCompleted { get; private set; }
    
    /// <summary>The time left for this stage. Zero means the stage is NOT timed.</summary>
    public float TimeLeft { get; private set; }
    
    /// <summary>
    /// Initializes a quest stage with one or more custom stage paths.
    /// </summary>
    /// <param name="id">Stage identifier (must be positive)</param>
    /// <param name="description">Description of the <see cref="QuestStage"/></param>
    /// <param name="paths">One or more predefined stage paths</param>
    public QuestStage(int id, string description, params StagePath[] paths) {
        // Argument checks
        ArgumentNullException.ThrowIfNull(description);
        ArgumentNullException.ThrowIfNull(paths);

        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Id must be positive");
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Must have a stage description", nameof(description));
        if (paths.Length == 0) throw new ArgumentException("At least one StagePath must be provided", nameof(paths));
        if (paths.Any(path => path == null)) throw new ArgumentException("Null StagePath found in input", nameof(paths));

        // Set fields
        Id = id;
        Description = description;
        _paths.AddRange(paths);
    }
    
    /// <summary>
    /// Tries to progress objective in the stage paths, based on the provided progress data.
    /// Marks the stage as completed if any path becomes completed.
    /// </summary>
    /// <param name="progressValue">The value of the progress made (e.g., 1 kill, 5 items gathered)</param>
    /// <param name="taskTypeId">The identifier of the action (e.g., Kill, Gather).</param>
    /// <param name="assetId">The id of the asset that was affected by the action (e.g., enemy ID)</param>
    /// <exception cref="InvalidOperationException"></exception>
    public void TryProgressStage(int progressValue, int taskTypeId, int assetId) {
        foreach (var path in _paths) {
            path.TryProgressPath(progressValue, taskTypeId, assetId);
            if(! path.IsCompleted) continue;
            IsCompleted = true;
            _nextStageId= path.NextStageId; // the completed stage path decides the next stage
        }
    }

    /// <summary>
    /// Gives a string representation of the overall stage progress
    /// </summary>
    /// <returns>Progress Description</returns>
    public string GetProgress()
    {
        string result = "";
        for (var i = 0; i < _paths.Count; i++)
        {
            var path = _paths[i];
            result += $"Path {i}:\n";
            var list = path.ObjectiveProgress;
            for (var j = 0; j < list.Count; j++)
            {
                var objectiveProg = list[j];
                result += $"objective {j}: {objectiveProg}\n";
            }
        }
        return result;
    }

    /// <summary>
    /// Returns the id of the stage tha comes after this one.
    /// An id of -1 means there is no next stage
    /// </summary>
    /// <returns>The id of the next stage in the quest</returns>
    /// <exception cref="QuestException">Thrown when the next stage has not been decided yet (i.e., when <c>_nextStageId</c> is 0).</exception>
    public int GetNextStageId() {
        if (_nextStageId == 0) throw new QuestException("Next stage has not been decided yet");
        return _nextStageId;
    }
    
    /// <summary>Completes the stage instantly. Useful for development </summary>
    public void CompleteInstantly() => IsCompleted = true;
    
    /// <summary>Marks the quest as timed</summary>
    public void MakeTimed(float newTime) => TimeLeft = newTime;
}
