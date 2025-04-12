using QuestSystem;
using QuestSystem.Entities;

namespace QuestSystemTests.Entities;

public class QuestStageInclusiveTests
{
    // SUT
    private readonly QuestStageInclusive _questStageInclusive;
    private readonly QuestStageInclusive _questStageInclusive2;

    public QuestStageInclusiveTests()
    {
        var taskKill = new Objective(5,(int)TaskType.Kill);
        var taskGather = new Objective(3,(int)TaskType.Gather);
        
        var taskKillFirst  = new Objective(7,(int)TaskType.Kill,10);
        var taskKillSecond = new Objective(5,(int)TaskType.Kill, 20);
        
        // Stage: 5 kills , 3 gathers
        _questStageInclusive = new QuestStageInclusive("kill and gather",taskKill, taskGather);
        
        // Stage: 7 kills of enemy id 10 , 5 kills of enemy id 20
        _questStageInclusive2 = new QuestStageInclusive("kill 2 things",taskKillFirst, taskKillSecond);
    }
    
    
    [Fact]
    public void QuestStage_ShouldInitializeWithTasks()
    {
        // Assert
        Assert.NotNull(_questStageInclusive);
        Assert.Equal("kill and gather",_questStageInclusive.StageDescription);
        Assert.Equal(2,_questStageInclusive.TotalObjectiveCount);
        Assert.Equal(0,_questStageInclusive.CompletedObjectiveCount);
        Assert.False(_questStageInclusive.IsCompleted);
    }

    [Fact]
    public void QuestStage_ShouldPrintProgressProperly()
    {
        // progress structs
        // Stage: 5 kills , 3 gathers
        var progressKill = new ObjectiveProgressDto((int)TaskType.Kill, 5);
        var progressGath = new ObjectiveProgressDto((int)TaskType.Gather, 3);
        
        Assert.Equal("0/2",_questStageInclusive.StageProgress);
        
        //progress stage
        _questStageInclusive.TryProgressTask(progressKill);
        Assert.Equal("1/2",_questStageInclusive.StageProgress);
        
        //finish stage
        _questStageInclusive.TryProgressTask(progressGath);
        Assert.Equal("2/2",_questStageInclusive.StageProgress);
        
    }
    
    [Fact]
    public void QuestStage_ShouldThrowExceptionWhenNoTasksProvided()
    {
        // Act & Assert
        var exception = Assert.Throws<Exception>(() => new QuestStageInclusive(""));
        Assert.Equal("No tasks are available", exception.Message);
    }
    
    [Fact]
    public void QuestStage_CanGetProgressOfStageObjectives()
    {
        // Stage: 5 kills , 3 gathers
        var progressKill = new ObjectiveProgressDto((int)TaskType.Kill, 2);
        var progressGath = new ObjectiveProgressDto((int)TaskType.Gather, 1);

        var progressList = _questStageInclusive.ObjectiveProgress;
        Assert.Equal("0/5",progressList[0]);
        Assert.Equal("0/3",progressList[1]);
        
        //progress stage
        _questStageInclusive.TryProgressTask(progressKill);
        progressList = _questStageInclusive.ObjectiveProgress;
        Assert.Equal("2/5",progressList[0]);
        Assert.Equal("0/3",progressList[1]);
        
        _questStageInclusive.TryProgressTask(progressGath);
        progressList = _questStageInclusive.ObjectiveProgress;
        Assert.Equal("2/5",progressList[0]);
        Assert.Equal("1/3",progressList[1]);
        
        _questStageInclusive.TryProgressTask(progressKill);
        _questStageInclusive.TryProgressTask(progressGath);
        _questStageInclusive.TryProgressTask(progressGath);
        progressList = _questStageInclusive.ObjectiveProgress;
        Assert.Equal("4/5",progressList[0]);
        Assert.Equal("3/3",progressList[1]);
        
        _questStageInclusive.TryProgressTask(progressKill);
        progressList = _questStageInclusive.ObjectiveProgress;
        Assert.Equal("5/5",progressList[0]);
        Assert.Equal("3/3",progressList[1]);

        Assert.True(_questStageInclusive.IsCompleted);
    }
    
    [Fact]
    public void QuestStage_ShouldThrowExceptionWhenNullTasksProvided()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new QuestStageInclusive("title",new Objective(5,(int)TaskType.Kill),null));
    }
    
    [Fact]
    public void TryProgressTask_ShouldProgressTask()
    {
        // Arrange
        var progressDto = new ObjectiveProgressDto((int)TaskType.Kill,10);
        
        // Act
        _questStageInclusive.TryProgressTask(progressDto);

        // Assert
        Assert.Equal(2,_questStageInclusive.TotalObjectiveCount);
        Assert.Equal(1,_questStageInclusive.CompletedObjectiveCount);
        Assert.False(_questStageInclusive.IsCompleted); // The second task is still incomplete, so the stage is not completed.
    }
    
    [Fact]
    public void TryProgressTask_ShouldProgressCorrectObjective()
    {
        var progressDtoIrrelevant = new ObjectiveProgressDto((int)TaskType.Kill,100,5);
        var progressDtoRelevant  = new ObjectiveProgressDto((int)TaskType.Kill,100,20);
        
        _questStageInclusive2.TryProgressTask(progressDtoIrrelevant);
        
        Assert.Equal(0,_questStageInclusive2.CompletedObjectiveCount);
        Assert.False(_questStageInclusive2.IsCompleted);
        
        _questStageInclusive2.TryProgressTask(progressDtoRelevant);
        
        Assert.Equal(1,_questStageInclusive2.CompletedObjectiveCount);
        Assert.False(_questStageInclusive2.IsCompleted); // The second task is still incomplete, so the stage is not completed.
    }
    
    [Fact]
    public void CheckStageCompletion_ShouldCompleteStageWhenAllTasksAreCompleted()
    {
        // Arrange
        var progressDto = new ObjectiveProgressDto((int)TaskType.Gather,10);
        var progressDto2 = new ObjectiveProgressDto((int)TaskType.Kill,10);

        // Act
        _questStageInclusive.TryProgressTask(progressDto);
        _questStageInclusive.TryProgressTask(progressDto2);

        // Assert
        Assert.Equal(2,_questStageInclusive.TotalObjectiveCount);
        Assert.Equal(2,_questStageInclusive.CompletedObjectiveCount);
        Assert.True(_questStageInclusive.IsCompleted);
    }
}