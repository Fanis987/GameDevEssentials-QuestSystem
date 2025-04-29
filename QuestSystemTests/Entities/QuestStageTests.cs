using QuestSystem;
using QuestSystem.Entities;

namespace QuestSystemTests.Entities;

public class QuestStageTests
{
    //Dependencies
    private List<Objective> _objList;
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
        _questStageInclusive = new QuestStage(1,"kill and gather",false,_objList);
        // Stage: 7 kills of enemy id 10 , 5 kills of enemy id 20
        _questStageInclusive2 = new QuestStage(2,"kill 2 things",false,objKillsList);
        // Stage: 5 kills OR 3 gathers
        _questStageSelective = new QuestStage(3,"kill or gather",true,_objList);
    }
    
    
    [Fact]
    public void QuestStage_ShouldInitializeWithObjectives()
    {
        // Normal stage
        Assert.NotNull(_questStageInclusive);
        Assert.Equal("kill and gather",_questStageInclusive.StageDescription);
        Assert.Equal(0f,_questStageInclusive.TimeLeft);
        Assert.Equal(1,_questStageInclusive.Id);
        Assert.False(_questStageInclusive.IsCompleted);
        
        // Selective Stage
        Assert.NotNull(_questStageSelective);
        Assert.Equal(0f,_questStageInclusive.TimeLeft);
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
        _questStageInclusive.TryProgressObjective(progressKill);
        Assert.Equal("1/2",_questStageInclusive.StageProgress);
        
        //finish stage
        _questStageInclusive.TryProgressObjective(progressGath);
        Assert.Equal("2/2",_questStageInclusive.StageProgress);
        
    }
    
    [Fact]
    public void QuestStage_ShouldThrowExceptionWhenNoTasksProvided() {
         Assert.Throws<ArgumentNullException>(() => new QuestStage(1,"stage",false,null));
    }
    
    [Fact]
    public void QuestStage_ShouldThrowExceptionWhenIdIsZero() {
        Assert.Throws<ArgumentOutOfRangeException>(() => new QuestStage(0,"stage",false,_objList));
    }
    [Fact]
    public void QuestStage_ShouldThrowExceptionWhenIdIsNegative() {
        Assert.Throws<ArgumentOutOfRangeException>(() => new QuestStage(-1,"stage",false,_objList));
    }
    
    [Fact]
    public void QuestStage_ShouldThrowExceptionWhenNoDescriptionIsProvided() {
        Assert.Throws<ArgumentException>(() => new QuestStage(2,"",false,_objList));
    }
    
    [Fact]
    public void QuestStage_ShouldThrowExceptionWhenEmptyListProvided() {
        Assert.Throws<ArgumentException>(() => new QuestStage(3,"",false,new List<Objective>()));
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
        _questStageInclusive.TryProgressObjective(progressKill);
        progressList = _questStageInclusive.ObjectiveProgress;
        Assert.Equal("2/5",progressList[0]);
        Assert.Equal("0/3",progressList[1]);
        
        _questStageInclusive.TryProgressObjective(progressGath);
        progressList = _questStageInclusive.ObjectiveProgress;
        Assert.Equal("2/5",progressList[0]);
        Assert.Equal("1/3",progressList[1]);
        
        _questStageInclusive.TryProgressObjective(progressKill);
        _questStageInclusive.TryProgressObjective(progressGath);
        _questStageInclusive.TryProgressObjective(progressGath);
        progressList = _questStageInclusive.ObjectiveProgress;
        Assert.Equal("4/5",progressList[0]);
        Assert.Equal("3/3",progressList[1]);
        
        _questStageInclusive.TryProgressObjective(progressKill);
        progressList = _questStageInclusive.ObjectiveProgress;
        Assert.Equal("5/5",progressList[0]);
        Assert.Equal("3/3",progressList[1]);

        Assert.True(_questStageInclusive.IsCompleted);
    }
    
    [Fact]
    public void QuestStage_ShouldThrowExceptionWhenNullTasksProvided()
    {
        var objList = new List<Objective>(){new Objective(5,(int)TaskType.Kill),null};
        Assert.Throws<ArgumentException>(() => new QuestStage(1,"title",false,objList));
    }
    
    [Fact]
    public void TryProgressObjective_ShouldProgressTask()
    {
        // Arrange
        var progressDto = new ObjectiveProgressDto((int)TaskType.Kill,10);
        // Act
        _questStageInclusive.TryProgressObjective(progressDto);
        // Assert
        Assert.Equal(1,_questStageInclusive.CompletedObjectiveCount);
        Assert.False(_questStageInclusive.IsCompleted); // The second task is still incomplete, so the stage is not completed.
    }
    
    [Fact]
    public void TryProgressObjective_ShouldProgressTask2()
    {
        // Act
        _questStageInclusive.TryProgressObjective(10,(int)TaskType.Kill);
        // Assert
        Assert.Equal(1,_questStageInclusive.CompletedObjectiveCount);
        Assert.False(_questStageInclusive.IsCompleted); // The second task is still incomplete, so the stage is not completed.
    }
    
    [Fact]
    public void TryProgressObjective_ShouldProgressCorrectObjective()
    {
        var progressDtoIrrelevant = new ObjectiveProgressDto((int)TaskType.Kill,100,5);
        var progressDtoRelevant  = new ObjectiveProgressDto((int)TaskType.Kill,100,20);
        
        _questStageInclusive2.TryProgressObjective(progressDtoIrrelevant);
        
        Assert.Equal(0,_questStageInclusive2.CompletedObjectiveCount);
        Assert.False(_questStageInclusive2.IsCompleted);
        
        _questStageInclusive2.TryProgressObjective(progressDtoRelevant);
        
        Assert.Equal(1,_questStageInclusive2.CompletedObjectiveCount);
        Assert.False(_questStageInclusive2.IsCompleted); // The second task is still incomplete, so the stage is not completed.
    }
    
    [Fact]
    public void TryProgressObjective_ShouldProgressSelective()
    {
        // Arrange
        var progressDto = new ObjectiveProgressDto((int)TaskType.Kill,2);
        
        //Act
        _questStageSelective.TryProgressObjective(progressDto);
            
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
        _questStageSelective.TryProgressObjective(progressDto);
            
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
        _questStageInclusive.TryProgressObjective(progressDto);
        _questStageInclusive.TryProgressObjective(progressDto2);

        // Assert
        //Assert.Equal(2,_questStageInclusive.TotalObjectiveCount);
        Assert.Equal(2,_questStageInclusive.CompletedObjectiveCount);
        Assert.True(_questStageInclusive.IsCompleted);
    }

    [Fact]
    public void MakeTimed_CanMakeTimed() {
        Assert.Equal(0f,_questStageInclusive.TimeLeft);
        _questStageInclusive.MakeTimed(2f);
        Assert.Equal(2f,_questStageInclusive.TimeLeft);
    }
    
    [Fact]
    public void ConnectToNextStages_SetsDefaultOnlyWhenOnlyDefaultProvided()
    {
        _questStageInclusive.ConnectToNextStages(10);

        Assert.Equal(10, _questStageInclusive.DefaultNextStageId);
        Assert.Equal(0, _questStageInclusive.AltNextStageIdFirst);
        Assert.Equal(0, _questStageInclusive.AltNextStageIdSecond);
    }

    [Fact]
    public void ConnectToNextStages_SetsAllIdsWhenAllProvided()
    {
        _questStageInclusive.ConnectToNextStages(5, 15, 25);

        Assert.Equal(5, _questStageInclusive.DefaultNextStageId);
        Assert.Equal(15, _questStageInclusive.AltNextStageIdFirst);
        Assert.Equal(25, _questStageInclusive.AltNextStageIdSecond);
    }
    
    [Fact]
    public void ConnectToNextStages_ThrowsExceptionWhenDefaultIsZeroOrNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            _questStageInclusive.ConnectToNextStages(0));

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            _questStageInclusive.ConnectToNextStages(-1));
    }

    [Fact]
    public void ConnectToNextStages_ThrowsExceptionWhenAltFirstIsNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            _questStageInclusive.ConnectToNextStages(1, -5));
    }
    
    [Fact]
    public void ConnectToNextStages_ThrowsExceptionWhenAltSecondIsNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            _questStageInclusive.ConnectToNextStages(1, 0, -10));
    }

    [Fact]
    public void ConnectToNextStages_AllowsZeroAltValues()
    {
        _questStageInclusive.ConnectToNextStages(3, 0, 0);

        Assert.Equal(3, _questStageInclusive.DefaultNextStageId);
        Assert.Equal(0, _questStageInclusive.AltNextStageIdFirst);
        Assert.Equal(0, _questStageInclusive.AltNextStageIdSecond);
    }
    

    [Fact]
    public void SetFinal_SetsDefaultNextStageIdToMinusOne()
    {
        _questStageInclusive.SetFinal();
        Assert.Equal(-1, _questStageInclusive.DefaultNextStageId);
    }

    
}