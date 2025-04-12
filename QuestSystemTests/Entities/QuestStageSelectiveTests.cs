using QuestSystem;
using QuestSystem.Entities;

namespace QuestSystemTests.Entities;

public class QuestStageSelectiveTests
{
    private readonly Objective _taskKill;
    private readonly Objective _taskGather;
    
    private QuestStageSelective _questStageSelective;

    public QuestStageSelectiveTests()
    {
        _taskKill = new Objective(5,(int)TaskType.Kill);
        _taskGather = new Objective(3,(int)TaskType.Gather);
        _questStageSelective = new QuestStageSelective("kill or gather",_taskKill, _taskGather);
    }

    [Fact]
    public void ShouldInitializeWithoutProgress()
    {
        Assert.NotNull(_questStageSelective);
        Assert.Equal(0,_questStageSelective.CompletedObjectiveCount);
        Assert.False(_questStageSelective.IsCompleted);
    }
    
    [Fact]
    public void ShouldProgress()
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
    public void ShouldCompleteWithOneTaskCompleted()
    {
        // Arrange
        var progressDto = new ObjectiveProgressDto((int)TaskType.Kill,5);
        
        //Act
        _questStageSelective.TryProgressTask(progressDto);
            
        //Assert
        Assert.True(_taskKill.IsCompleted);
        Assert.NotNull(_questStageSelective);
        Assert.Equal(1,_questStageSelective.CompletedObjectiveCount);
        Assert.True(_questStageSelective.IsCompleted);
    }
}