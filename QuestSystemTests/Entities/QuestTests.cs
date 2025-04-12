using QuestSystem;
using QuestSystem.Entities;

namespace QuestSystemTests.Entities;

public class QuestTests
{
    
    // Dependencies
    private readonly Objective _taskKill,_taskKillLarge,_taskGather;
    private readonly QuestStage _questStage1,_questStage2;
    
    // SUT
    private readonly Quest _quest;
    
    public QuestTests()
    {
        // Dependencies
        _taskKill = new Objective(5,(int)TaskType.Kill);
        _taskKillLarge = new Objective(10,(int)TaskType.Kill);
        _taskGather = new Objective(3,(int)TaskType.Gather);
        _questStage1 = new QuestStageInclusive("kill and gather",_taskKill, _taskGather);
        _questStage2 = new QuestStageInclusive("kill many",_taskKillLarge);
        
        // SUT
        _quest = new Quest(1,"myQuest",_questStage1,_questStage2);
    }
    
    [Fact]
    public void Quest_ShouldInitializeProperly()
    {
        // Assert
        Assert.Equal(0,_taskKill.CurrValue);
        Assert.False(_taskKill.IsCompleted);
        Assert.Equal(0,_taskKillLarge.CurrValue);
        Assert.False(_taskKillLarge.IsCompleted);
        Assert.Equal(0,_taskGather.CurrValue);
        Assert.False(_taskGather.IsCompleted);
        
        Assert.False(_questStage1.IsCompleted);
        Assert.False(_questStage2.IsCompleted);
        
        // Quest assertions
        Assert.NotNull(_quest);
        Assert.Equal("myQuest",_quest.Title);
        Assert.Equal(1,_quest.Id);
        Assert.False(_quest.IsCompleted);
        Assert.Equal(2, _quest.StagesLeft);
        Assert.Equal(_questStage1, _quest.CurrentStage);
        
        //Alt COnstructor assertions
        var quest2 = new Quest(3, "title", true,
            "stageDescr", _taskGather,_taskKill);
        Assert.NotNull(quest2);
        Assert.Equal("title",quest2.Title);
        Assert.Equal(3,quest2.Id);
        Assert.False(quest2.IsCompleted);
        Assert.Equal(1, quest2.StagesLeft);
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
    public void Quest_ShouldThrowExceptionForEmptyTitle()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Quest(1,"",_questStage1));
        Assert.Throws<ArgumentException>(() => new Quest(1,"",true,"stageDescr",_taskGather));
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
    public void Quest_ShouldThrowExceptionForNullStageInStages()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Quest(1,"myQuest",new QuestStageInclusive("",null)));
    }
    
    [Fact]
    public void TryProgressQuest_ShouldProgressCurrentStage()
    {
        //Arrange
        var objectiveProgressDto = new ObjectiveProgressDto((int)TaskType.Kill,3);
        
        // Act
        _quest.TryProgressQuest(objectiveProgressDto);

        // Assert
        Assert.Equal(_questStage1, _quest.CurrentStage);
        Assert.Equal(3,_taskKill.CurrValue);
        Assert.False(_questStage1.IsCompleted);
    }
    
    [Fact]
    public void TryProgressQuest_ShouldMoveToNextStageAfterCompletion()
    {
        //Arrange
        var taskProgressDto  = new ObjectiveProgressDto((int)TaskType.Kill,5);
        var taskProgressDto2 = new ObjectiveProgressDto((int)TaskType.Gather,5);
        
        // Act
        _quest.TryProgressQuest(taskProgressDto);
        _quest.TryProgressQuest(taskProgressDto2);

        // Assert
        Assert.True(_questStage1.IsCompleted);
        Assert.Equal(_questStage2, _quest.CurrentStage);
        Assert.Equal(1, _quest.StagesLeft);
    }
    
    [Fact]
    public void TryProgressQuest_ShouldMarkQuestAsCompleted()
    {
        //Arrange
        var taskProgressDto = new ObjectiveProgressDto((int)TaskType.Kill,5);
        var taskProgressDto2 = new ObjectiveProgressDto((int)TaskType.Gather,5);
        
        // Act - Assert
        _quest.TryProgressQuest(taskProgressDto);
        Assert.Equal(_quest.CurrentStage,_questStage1);
        Assert.False(_questStage1.IsCompleted);
        
        _quest.TryProgressQuest(taskProgressDto2);
        Assert.True(_questStage1.IsCompleted);
        Assert.Equal(_quest.CurrentStage,_questStage2);
        Assert.False(_questStage2.IsCompleted);
    }
    
    [Fact]
    public void TryProgressQuest_ShouldNotProceedCompletedQuest()
    {
        //Complete the quest
        var taskProgressDto = new ObjectiveProgressDto((int)TaskType.Kill,5);
        var taskProgressDto2 = new ObjectiveProgressDto((int)TaskType.Gather,5);
        _quest.TryProgressQuest(taskProgressDto);
        Assert.False(_quest.CurrentStage.IsCompleted);
        Assert.Equal(2, _quest.StagesLeft);
        Assert.False(_quest.IsCompleted);
        
        _quest.TryProgressQuest(taskProgressDto2);
        Assert.Equal(1, _quest.StagesLeft);
        Assert.False(_quest.IsCompleted);
        
        var taskProgressDto3 = new ObjectiveProgressDto((int)TaskType.Kill,15);
        _quest.TryProgressQuest(taskProgressDto3);
        Assert.Equal(0, _quest.StagesLeft);
        Assert.True(_quest.IsCompleted);
        
        //Try to proceed past the end - nothing happens
        _quest.TryProgressQuest(taskProgressDto3);
        Assert.Equal(0, _quest.StagesLeft);
        Assert.True(_quest.IsCompleted);
    }
}