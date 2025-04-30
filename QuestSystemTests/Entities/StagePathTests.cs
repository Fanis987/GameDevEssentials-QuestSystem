using QuestSystem.Entities;

namespace QuestSystemTests.Entities;

public class StagePathTests
{
    // Dependencies
    private readonly Objective _killTask;
    private readonly Objective _gatherTask;
    private readonly Objective _completedTask;
    private readonly Objective _uncompletedTask;

    // SUT
    private StagePath _stagePathInclusive;
    private StagePath _stagePathSelective;

    public StagePathTests()
    {
        _killTask = new Objective(5, (int)TaskType.Kill);
        _gatherTask = new Objective(3, (int)TaskType.Gather);
        _completedTask = new Objective(1, (int)TaskType.Gather);
        _completedTask.TryProceed(1, (int)TaskType.Gather); //complete it
        _uncompletedTask = new Objective(1, (int)TaskType.Gather);

        _stagePathInclusive = new StagePath(false, _killTask, _gatherTask);
        _stagePathSelective = new StagePath(true, _killTask, _gatherTask);
    }

    [Fact]
    public void StagePath_ShouldInitialize_WithValidObjectives() {
        Assert.NotNull(_stagePathInclusive);
        Assert.Equal("0/2", _stagePathInclusive.StageProgress);
        Assert.False(_stagePathInclusive.IsCompleted);
        Assert.False(_stagePathInclusive.IsSelective);
        
        Assert.NotNull(_stagePathSelective);
        Assert.Equal("0/2", _stagePathSelective.StageProgress);
        Assert.False(_stagePathSelective.IsCompleted);
        Assert.True(_stagePathSelective.IsSelective);
    }

    [Fact]
    public void StagePath_Throws_IfObjectiveArrayIsNull() {
        Objective[] nullObjectives = null;
        Assert.Throws<ArgumentNullException>(() => new StagePath(false, nullObjectives));
    }

    [Fact]
    public void StagePath_Throws_IfObjectiveArrayIsEmpty() {
        Assert.Throws<ArgumentException>(() => new StagePath(false));
    }

    [Fact]
    public void StagePath_Throws_IfAnyObjectiveIsNull() {
        Objective obj1 = new Objective(1, 0);
        Objective obj2 = null;
        Assert.Throws<ArgumentNullException>(() => new StagePath(false, obj1, obj2));
    }
    
    [Fact]
    public void TryProgressObjective_ProgressesOnlyMatchingTaskTypes() {
        _stagePathInclusive.TryProgressObjective(3, (int)TaskType.Kill);

        Assert.Equal(3, _killTask.CurrValue);
        Assert.Equal(0, _gatherTask.CurrValue);
    }

    [Fact]
    public void TryProgressObjective_IgnoresWrongTaskTypes() {
        _stagePathInclusive.TryProgressObjective(5, 999); // Nonexistent TaskType

        Assert.Equal(0, _killTask.CurrValue);
        Assert.Equal(0, _gatherTask.CurrValue);
    }

    [Fact]
    public void TryProgressObjective_TriggersCompletion_Inclusive_AllObjectives() {
        _stagePathInclusive.TryProgressObjective(5, (int)TaskType.Kill);
        _stagePathInclusive.TryProgressObjective(3, (int)TaskType.Gather);

        Assert.True(_stagePathInclusive.IsCompleted);
    }

    [Fact]
    public void TryProgressObjective_TriggersCompletion_Selective_AnyObjective() {
        _stagePathSelective.TryProgressObjective(5, (int)TaskType.Kill);

        Assert.True(_stagePathSelective.IsCompleted);
    }

    [Fact]
    public void TryProgressObjective_RespectsAssetFiltering() {
        var objTargeted = new Objective(2, (int)TaskType.Kill, assetId: 42);
        var objIgnored  = new Objective(2, (int)TaskType.Kill, assetId: 99);

        var stage = new StagePath(false, objTargeted, objIgnored);

        stage.TryProgressObjective(2, (int)TaskType.Kill, assetId: 42);

        Assert.Equal(2, objTargeted.CurrValue);
        Assert.Equal(0, objIgnored.CurrValue);
    }

    [Fact]
    public void StagePath_SelectiveCompletesIfAnyObjectiveCompletesThroughProgress()
    {
        var obj1 = new Objective(3, (int)TaskType.Gather);
        var obj2 = new Objective(5, (int)TaskType.Kill);

        var stage = new StagePath(true, obj1, obj2);

        // Progress only the first objective
        stage.TryProgressObjective(3, (int)TaskType.Gather);

        Assert.True(obj1.IsCompleted);
        Assert.False(obj2.IsCompleted);
        Assert.True(stage.IsCompleted); // Selective: one completed is enough
    }

    [Fact]
    public void StagePath_InclusiveCompletesOnlyIfAllObjectivesCompletedThroughProgress()
    {
        var obj1 = new Objective(2, (int)TaskType.Kill);
        var obj2 = new Objective(3, (int)TaskType.Gather);

        var stage = new StagePath(false, obj1, obj2);

        // Progress only one: not enough
        stage.TryProgressObjective(2, (int)TaskType.Kill);
        Assert.False(stage.IsCompleted);

        // Progress the other one
        stage.TryProgressObjective(3, (int)TaskType.Gather);
        Assert.True(stage.IsCompleted);
    }

    [Fact]
    public void ObjectiveProgress_ReturnsNonEmptyProgressStrings()
    {
        var obj1 = new Objective(5, (int)TaskType.Kill);
        var obj2 = new Objective(3, (int)TaskType.Gather);

        var stage = new StagePath(false, obj1, obj2);

        var progressStrings = stage.ObjectiveProgress;

        Assert.Equal(2, progressStrings.Count);
        Assert.All(progressStrings, s => Assert.False(string.IsNullOrWhiteSpace(s)));
    }
}