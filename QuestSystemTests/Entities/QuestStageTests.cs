using QuestSystem;
using QuestSystem.Entities;

namespace QuestSystemTests.Entities;

public class QuestStageTests
{
    //Dependencies
    private new List<Objective> _objList;
    // SUT
    private readonly QuestStage _questStageInclusive;
    private readonly QuestStage _questStageSelective;
    private readonly QuestStage _questStageInclusive2;

    public QuestStageTests()
    {
        var taskKill = new Objective(5,(int)TaskType.Kill);
        var taskGather = new Objective(3,(int)TaskType.Gather);
        _objList = new List<Objective>(){taskKill, taskGather};
        
        var taskKillFirst  = new Objective(7,(int)TaskType.Kill,10);
        var taskKillSecond = new Objective(5,(int)TaskType.Kill, 20);
        var objKillsList = new List<Objective>(){taskKillFirst, taskKillSecond};
        
        // Stage: 5 kills AND 3 gathers
        _questStageInclusive = new QuestStage("kill and gather",false,_objList);
        // Stage: 7 kills of enemy id 10 , 5 kills of enemy id 20
        _questStageInclusive2 = new QuestStage("kill 2 things",false,objKillsList);
        // Stage: 5 kills OR 3 gathers
        _questStageSelective = new QuestStage("kill or gather",true,_objList);
    }
    
    
    [Fact]
    public void QuestStage_ShouldInitializeWithObjectives()
    {
        // Normal stage
        Assert.NotNull(_questStageInclusive);
        Assert.Equal("kill and gather",_questStageInclusive.StageDescription);
        Assert.False(_questStageInclusive.IsCompleted);
        
        // Selective Stage
        Assert.NotNull(_questStageSelective);
        Assert.Equal(0,_questStageSelective.CompletedObjectiveCount);
        Assert.False(_questStageSelective.IsCompleted);
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
    public void QuestStage_ShouldThrowExceptionWhenNoTasksProvided() {
        var exception = Assert.Throws<ArgumentNullException>(() => new QuestStage("stage",false,null));
    }
    
    [Fact]
    public void QuestStage_ShouldThrowExceptionWhenNoDescriptionIsProvided() {
        var exception = Assert.Throws<ArgumentException>(() => new QuestStage("",false,_objList));
    }
    
    [Fact]
    public void QuestStage_ShouldThrowExceptionWhenEmptyListProvided() {
        var exception = Assert.Throws<ArgumentException>(() => new QuestStage("",false,new List<Objective>()));
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
        var objList = new List<Objective>(){new Objective(5,(int)TaskType.Kill),null};
        Assert.Throws<ArgumentException>(() => new QuestStage("title",false,objList));
    }
    
    [Fact]
    public void TryProgressTask_ShouldProgressTask()
    {
        // Arrange
        var progressDto = new ObjectiveProgressDto((int)TaskType.Kill,10);
        
        // Act
        _questStageInclusive.TryProgressTask(progressDto);

        // Assert
        //Assert.Equal(2,_questStageInclusive.TotalObjectiveCount);
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
    public void TryProgressTask_ShouldProgressSelective()
    {
        // Arrange
        var progressDto = new ObjectiveProgressDto((int)TaskType.Kill,2);
        
        //Act
        _questStageSelective.TryProgressTask(progressDto);
            
        //Assert
        Assert.NotNull(_questStageSelective);
        Assert.Equal(0,_questStageSelective.CompletedObjectiveCount);
        Assert.False(_questStageSelective.IsCompleted);
    }
    
    [Fact]
    public void CheckStageCompletion_ShouldCompleteWithAnyTaskCompleted()
    {
        // Arrange
        var progressDto = new ObjectiveProgressDto((int)TaskType.Kill,5);
        
        //Act
        _questStageSelective.TryProgressTask(progressDto);
            
        //Assert
        Assert.NotNull(_questStageSelective);
        Assert.True(_questStageSelective.IsCompleted);
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
        //Assert.Equal(2,_questStageInclusive.TotalObjectiveCount);
        Assert.Equal(2,_questStageInclusive.CompletedObjectiveCount);
        Assert.True(_questStageInclusive.IsCompleted);
    }
}