using QuestSystem;
using QuestSystem.Entities;

namespace QuestSystemTests.Entities;

public class QuestStageTests
{
    // Dependencies
    private readonly Objective _taskKill;
    private readonly Objective _taskGather;
    private readonly Objective _taskKillFirst;
    private readonly Objective _taskKillSecond;
    private readonly StagePath _pathInclusive;
    // SUT (System Under Test)
    private readonly QuestStage _questStageInclusive;
    private readonly QuestStage _questStageSelective;
    private readonly QuestStage _questStageInclusive2;

    public QuestStageTests()
    {
        // Setup objectives
        _taskKill       = new Objective(5, (int)TaskType.Kill);
        _taskGather     = new Objective(3, (int)TaskType.Gather);
        _taskKillFirst  = new Objective(7, (int)TaskType.Kill, 10);
        _taskKillSecond = new Objective(5, (int)TaskType.Kill, 20);

        // Create StagePaths
        _pathInclusive = new StagePath(false, _taskKill, _taskGather); // All objectives must be completed
        var pathSelective = new StagePath(true, _taskKill, _taskGather);  // Any objective completion will suffice
        var pathInclusive2 = new StagePath(false, _taskKillFirst, _taskKillSecond); // Specific kills for two enemies

        // Stage: 5 kills AND 3 gathers
        _questStageInclusive = new QuestStage(1, "kill and gather", _pathInclusive);

        // Stage: 7 kills of enemy id 10 AND 5 kills of enemy id 20
        _questStageInclusive2 = new QuestStage(2, "kill 2 things", pathInclusive2);

        // Stage: 5 kills OR 3 gathers
        _questStageSelective = new QuestStage(3, "kill or gather", pathSelective);
    }

    [Fact]
    public void QuestStage_ShouldInitializeWithPaths()
    {
        // Assert that stages are initialized correctly with paths
        Assert.NotNull(_questStageInclusive);
        Assert.NotNull(_questStageInclusive2);
        Assert.NotNull(_questStageSelective);

        // Assert stage description and ids
        Assert.Equal("kill and gather", _questStageInclusive.StageDescription);
        Assert.Equal(1, _questStageInclusive.Id);
        Assert.Equal("kill 2 things", _questStageInclusive2.StageDescription);
        Assert.Equal(2, _questStageInclusive2.Id);
        Assert.Equal("kill or gather", _questStageSelective.StageDescription);
        Assert.Equal(3, _questStageSelective.Id);
    }
    [Fact]
    public void QuestStage_ShouldThrowArgumentOutOfRangeException_WhenIdIsZeroOrNegative() {
        Assert.Throws<ArgumentOutOfRangeException>(() => { var q = new QuestStage(0, "Valid Description", _pathInclusive); });
        Assert.Throws<ArgumentOutOfRangeException>(() => { var q = new QuestStage(-1, "Valid Description", _pathInclusive); });
    }
    
    [Fact]
    public void QuestStage_ShouldThrowArgumentNullExceptionWhenStageDescriptionIsNull() {
        Assert.Throws<ArgumentNullException>(() => { var q = new QuestStage(1, null, _pathInclusive); });
    }

    [Fact]
    public void QuestStage_ShouldThrowArgumentException_WhenStageDescriptionIsEmpty() {
        Assert.Throws<ArgumentException>(() => { var q =new QuestStage(1, "", _pathInclusive);});
    }
    
    [Fact]
    public void QuestStage_ShouldThrowArgumentException_WhenNullPathIsInPathsArray() { 
        Assert.Throws<ArgumentException>(() => 
            { var q =new QuestStage(1, "Valid Description", _pathInclusive, null); });
    }
    
    [Fact]
    public void QuestStage_ShouldThrowArgumentNullExceptionWhenPathsAreNull() {
        Assert.Throws<ArgumentNullException>(() => 
            { var q = new QuestStage(1, "Valid Description", (StagePath[])null); });
    }
    
    [Fact]
    public void QuestStage_ShouldThrowArgumentException_WhenNoPathsProvided() {
        Assert.Throws<ArgumentException>(() => { var q =new QuestStage(1, "Valid Description"); });
    }
    
    [Fact]
    public void QuestStage_ShouldBeCompleted_WhenAllObjectivesComplete_Inclusive() {
        _questStageInclusive.TryProgressObjective(5, (int)TaskType.Kill);  // Progress the Kill objective
        _questStageInclusive.TryProgressObjective(3, (int)TaskType.Gather); // Progress the Gather objective

        Assert.True(_questStageInclusive.IsCompleted); // Both objectives should be completed, so the stage is complete
    }

    [Fact]
    public void QuestStage_ShouldBeCompleted_WhenAnyObjectiveComplete_Selective()
    {
        _questStageSelective.TryProgressObjective(5, (int)TaskType.Kill);  // Progress the Kill task
        
        Assert.True(_questStageSelective.IsCompleted); // The stage should be marked as completed as soon as one objective is completed
    }

    [Fact]
    public void QuestStage_ShouldNotBeCompleted_IfNoObjectivesComplete() {
        Assert.False(_questStageSelective.IsCompleted);
    }

    [Fact]
    public void QuestStage_ShouldHandleTimedStages()
    {
        _questStageInclusive.MakeTimed(120f);
        Assert.Equal(120f, _questStageInclusive.TimeLeft);
    }
}