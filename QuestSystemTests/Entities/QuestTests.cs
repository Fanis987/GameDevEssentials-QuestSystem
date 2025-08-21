using QuestSystem;
using QuestSystem.Entities;

namespace QuestSystemTests.Entities;

public class QuestTests
{
    
    // Dependencies
    private readonly Objective _taskKill,_taskKillLarge,_taskGather;
    private readonly StagePath _stagePathInclusive,_stagePathInclusiveL,_stagePathSelective;
    private readonly QuestStage _questStage1,_questStage2,_questStage3;
    
    // SUT
    private readonly Quest _quest,_quest2,_questSelective,_questFailed;

    public QuestTests()
    {
        // Dependencies
        _taskKill = new Objective(5,(int)TaskType.Kill);
        _taskKillLarge = new Objective(10,(int)TaskType.Kill);
        _taskGather = new Objective(3,(int)TaskType.Gather);

        _stagePathInclusive = new StagePath(false,2, _taskKill, _taskGather);
        _stagePathInclusiveL= new StagePath(false,-1, _taskKillLarge);
        _stagePathSelective = new StagePath(true,-1, _taskKill, _taskGather);
        
        _questStage1 = new QuestStage(1,"kill and gather",_stagePathInclusive);
        _questStage2 = new QuestStage(2,"kill many",_stagePathInclusiveL);
        _questStage3 = new QuestStage(3,"kill or gather",_stagePathSelective);
        
        // SUT
        _quest = new Quest(1,"myQuest",_questStage1,_questStage2);
        _questFailed = new Quest(1,"myQuest",_questStage1,_questStage2);
        _quest2 = new Quest(2,"myQuest2",true,_questStage1,_questStage2);
        _questSelective = new Quest(3,"myQuest3",_questStage3);
    }
    
    [Fact]
    public void Quest_ShouldInitializeProperly()
    {
        // Assert Dependencies
        Assert.Equal(0,_taskKill.CurrValue);
        Assert.False(_taskKill.IsCompleted);
        Assert.Equal(0,_taskKillLarge.CurrValue);
        Assert.False(_taskKillLarge.IsCompleted);
        Assert.Equal(0,_taskGather.CurrValue);
        Assert.False(_taskGather.IsCompleted);
        
        Assert.False(_questStage1.IsCompleted);
        Assert.False(_questStage2.IsCompleted);
        Assert.False(_questStage3.IsCompleted);
        
        // Quest assertions
        Assert.NotNull(_quest);
        Assert.Equal("myQuest",_quest.Title);
        Assert.Equal(1,_quest.Id);
        Assert.False(_quest.IsCompleted);
        Assert.False(_quest.IsMainQuest);
        Assert.Equal(0, _quest.NextQuestId);
        Assert.Equal(_questStage1, _quest.CurrentStage);
        
        // Quest selective assertions
        Assert.NotNull(_questSelective);
        Assert.Equal("myQuest3",_questSelective.Title);
        Assert.Equal(3,_questSelective.Id);
        Assert.False(_questSelective.IsCompleted);
        Assert.False(_questSelective.WasFailed);
    }

    [Fact]
    public void Quest_ShouldInitializeProperlyWithAltCtor()
    {
        //Single-stage Constructor assertions
        var quest = new Quest(1, "title", true,
            "stageDescr", _taskGather,_taskKill);
        Assert.NotNull(quest);
        Assert.Equal(1,quest.Id);
        Assert.Equal("title",quest.Title);
        Assert.False(quest.IsCompleted);
            
        var quest2 = new Quest(3, "title", false,
            "stageDescr", _taskGather,_taskKill);
        Assert.NotNull(quest2);
        Assert.Equal("title",quest2.Title);
        Assert.Equal(3,quest2.Id);
        Assert.False(quest2.IsCompleted);
    }
    
    [Fact]
    public void Quest_ShouldInitializeProperlyWithAltCtor2() {
        // Quest assertions
        Assert.NotNull(_quest2);
        Assert.Equal("myQuest2",_quest2.Title);
        Assert.Equal(2,_quest2.Id);
        Assert.False(_quest2.IsCompleted);
        Assert.True(_quest2.IsMainQuest);
        Assert.Equal(_questStage1, _quest2.CurrentStage);
    }
    
    [Fact]
    public void Quest_ShouldThrowExceptionForNegativeId()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Quest(-1,"myQuest",_questStage1));
        Assert.Throws<ArgumentException>(() => new Quest(-1,"myQuest",true,"stageDescr",_taskGather));
    }
    
    [Fact]
    public void Quest_ShouldThrowExceptionForZeroId()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Quest(0,"myQuest",_questStage1));
        Assert.Throws<ArgumentException>(() => new Quest(0,"myQuest",true,"stageDescr",_taskGather));
    }
    
    [Fact]
    public void Quest_ShouldThrowExceptionForNullStages()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Quest(1,"title",true,"stageDescr",null));
    }
    
    [Fact]
    public void Quest_ShouldThrowExceptionForNullTitle()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Quest(1,null,_questStage1));
        Assert.Throws<ArgumentException>(() => new Quest(1,null,true,"stageDescr",_taskGather));
    }
    
    [Fact]
    public void Quest_ShouldThrowExceptionForLongTitle() {
        // Act & Assert
        string longTitle = new string('a', 201);
        Assert.Throws<ArgumentException>(() => new Quest(1,longTitle,_questStage1));
        Assert.Throws<ArgumentException>(() => new Quest(1,longTitle,true,"stageDescr",_taskGather));
    }
    
    [Fact]
    public void Quest_ShouldThrowExceptionNoDescription() {
        Assert.Throws<ArgumentException>(() => new Quest(1,"Title",true,"",_taskGather));
        Assert.Throws<ArgumentException>(() => new Quest(1,"Title",true,null,_taskGather));
    }
    
    [Fact]
    public void Quest_ShouldThrowExceptionForLongDescr() {
        // Act & Assert
        string longDescr = new string('a', 2001);
        Assert.Throws<ArgumentException>(() => new Quest(1,"Title",true,longDescr,_taskGather));
    }
    
    [Fact]
    public void Quest_ShouldThrowExceptionForDuplicateStageIds()
    {
        var sameIdStage = new QuestStage(1,"title",_stagePathInclusive);
        // Act & Assert
        Assert.Throws<QuestException>(() => new Quest(7,"a title",_questStage1,sameIdStage));
    }
    
    
    [Fact]
    public void TryProgressQuest_ShouldProgressCurrentStage()
    {
        // Act
        _quest.TryProgressQuest(3,(int)TaskType.Kill);

        // Assert
        Assert.Equal(_questStage1, _quest.CurrentStage);
        Assert.Equal(3,_taskKill.CurrValue);
        Assert.False(_questStage1.IsCompleted);
    }
    
    [Fact]
    public void TryProgressQuest_ShouldMoveToNextStageAfterCompletion()
    {
        // Act
        _quest.TryProgressQuest(5,(int)TaskType.Kill);
        _quest.TryProgressQuest(5,(int)TaskType.Gather);

        // Assert
        Assert.True(_questStage1.IsCompleted);
        Assert.Equal(_questStage2, _quest.CurrentStage);
    }
    
    [Fact]
    public void TryProgressQuest_ShouldMarkQuestAsCompleted() {
        // Act - Assert
        _quest.TryProgressQuest(5,(int)TaskType.Kill);
        Assert.Equal(_quest.CurrentStage,_questStage1);
        Assert.False(_questStage1.IsCompleted);
        
        _quest.TryProgressQuest(5,(int)TaskType.Gather);
        Assert.True(_questStage1.IsCompleted);
        Assert.Equal(_quest.CurrentStage,_questStage2);
        Assert.False(_questStage2.IsCompleted);
    }
    
    [Fact]
    public void TryProgressQuest_CannotProgressFailedQuest() {
        // Act - Assert
        _quest.Fail();
        _quest.TryProgressQuest(5,(int)TaskType.Kill);
        _quest.TryProgressQuest(5,(int)TaskType.Gather);
        Assert.False(_questStage1.IsCompleted);
    }
    
    [Fact]
    public void TryProgressQuest_ShouldMarkQuestAsCompleted2()
    {
        // Act - Assert
        _quest.TryProgressQuest(5,(int)TaskType.Kill,-1);
        Assert.Equal(_quest.CurrentStage,_questStage1);
        Assert.False(_questStage1.IsCompleted);
        
        _quest.TryProgressQuest(5,(int)TaskType.Gather,-1);
        Assert.True(_questStage1.IsCompleted);
        Assert.Equal(_quest.CurrentStage,_questStage2);
        Assert.False(_questStage2.IsCompleted);
    }
    
    [Fact]
    public void TryProgressQuest_ShouldNotProceedCompletedQuest()
    {
        //Complete the quest
        _quest.TryProgressQuest(5,(int)TaskType.Kill);
        Assert.False(_quest.CurrentStage.IsCompleted);
        Assert.False(_quest.IsCompleted);
        
        _quest.TryProgressQuest(5,(int)TaskType.Gather);
        Assert.False(_quest.IsCompleted);
        
        _quest.TryProgressQuest(15,(int)TaskType.Kill);
        Assert.True(_quest.IsCompleted);
        
        //Try to proceed past the end - nothing happens
        _quest.TryProgressQuest(15,(int)TaskType.Kill);
        Assert.True(_quest.IsCompleted);
    }
    
    [Fact]
    public void TryProgressQuest_ShouldThrowExceptionForCurrentStageCompleted()
    {
        //complete the stage and THEN create the quest
        _questStage1.TryProgressStage(5,(int)TaskType.Kill);
        _questStage1.TryProgressStage(3,(int)TaskType.Gather);
        Assert.True(_questStage1.IsCompleted);
        
        var quest = new Quest(2,"myQuest2",true,_questStage1);
        Assert.True(quest.CurrentStage.IsCompleted);
        Assert.False(quest.IsCompleted);

        Assert.Throws<InvalidOperationException>(() =>
            quest.TryProgressQuest(5,(int)TaskType.Kill));
    }
    
    
    [Fact]
    public void CompleteInstantly_CanCompleteInstantly() {
        Assert.False(_quest.IsCompleted);
        _quest.CompleteInstantly();
        Assert.True(_quest.IsCompleted);
    }
    
    [Fact]
    public void Fail_CanFailQuest() {
        Assert.False(_quest.WasFailed);
        _quest.Fail();
        Assert.True(_quest.WasFailed);
    }
}