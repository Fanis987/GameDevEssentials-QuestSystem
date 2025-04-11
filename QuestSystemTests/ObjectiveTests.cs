using QuestSystem;
using QuestSystem.Entities;


namespace QuestSystemTests;

public class ObjectiveTests
{
    //SUT
    private readonly Objective _objective,_assetObjective;

    public ObjectiveTests()
    {
        _objective = new Objective(10, (int)TaskType.Kill);
        //proceeds with specific assetId
        //COllect 5 instances of assetId=4
        _assetObjective = new Objective(5, (int)TaskType.Collect, 4); 
    }
    
    [Fact]
    public void ShouldInitializeUncompleted()
    {
        // Assert
        Assert.Equal(0,_objective.CurrValue);
        Assert.Equal(10,_objective.GoalValue);
        Assert.False(_objective.IsCompleted);
    }

    [Fact]
    public void ShouldNonReduceProgressBelowZero()
    {
       _assetObjective.TryProceed(-2);
       
       Assert.Equal(0,_objective.CurrValue);
       Assert.Equal(10,_objective.GoalValue);
       Assert.False(_objective.IsCompleted);
    }
    
    [Fact]
    public void CanShowProgress()
    {
        // Start with zero
        Assert.Equal(0,_assetObjective.CurrValue);
        Assert.Equal(5,_assetObjective.GoalValue);
        Assert.Equal("0/5",_assetObjective.ProgressPrint);
        Assert.False(_assetObjective.IsCompleted);
        
        // passing incorrect asset id - progress stays 0
        _assetObjective.TryProceed(2,7);
        Assert.Equal(0,_assetObjective.CurrValue);
        Assert.Equal("0/5",_assetObjective.ProgressPrint);
        
        // passing correct asset id - progress moves
        _assetObjective.TryProceed(2,4);
        Assert.Equal(2,_assetObjective.CurrValue);
        Assert.Equal("2/5",_assetObjective.ProgressPrint);
        
        // passing correct asset id - progress moves
        _assetObjective.TryProceed(1,4);
        Assert.Equal("3/5",_assetObjective.ProgressPrint);
        
        // finishing
        _assetObjective.TryProceed(2,4);
        Assert.Equal("5/5",_assetObjective.ProgressPrint);
        Assert.True(_assetObjective.IsCompleted);
        
        //trying to go over the end
        _assetObjective.TryProceed(2,4);
        Assert.Equal("5/5",_assetObjective.ProgressPrint);
        Assert.True(_assetObjective.IsCompleted);
    }
    
    [Fact]
    public void TryProceed_CanProgress()
    {
        // Act - Assert
        Assert.Equal(0,_assetObjective.CurrValue);
        
        // passing incorrect asset id - progress stays 0
        _assetObjective.TryProceed(2,7);
        Assert.Equal(0,_objective.CurrValue);
        Assert.False(_objective.IsCompleted);
        
        // passing correct asset id - progress moves
        _objective.TryProceed(2,4);
        Assert.Equal(2,_objective.CurrValue);
        Assert.False(_objective.IsCompleted);
    }
    
    [Fact]
    public void CanProgressOnlyWithCorrectAsset()
    {
        // Act - Assert
        Assert.Equal(0,_objective.CurrValue);
        
        _objective.TryProceed(2);
        Assert.Equal(2,_objective.CurrValue);
        Assert.False(_objective.IsCompleted);
        
        _objective.TryProceed(2);
        Assert.Equal(4,_objective.CurrValue);
        Assert.False(_objective.IsCompleted);
        
        _objective.TryProceed(6);
        Assert.Equal(10,_objective.CurrValue);
        Assert.True(_objective.IsCompleted);
    }


    [Fact]
    public void ShouldNotCompleteTaskIfRequirementIsNotMet()
    {
        // Act
        _objective.TryProceed(4);

        // Assert
        Assert.Equal(4,_objective.CurrValue);
        Assert.False(_objective.IsCompleted);
    }

    [Fact]
    public void ShouldNotAllowProgressAfterCompletion()
    {
        // Arrange - Assert
        _objective.TryProceed(10);
        var completed = _objective.IsCompleted;

        // Act
        _objective.TryProceed(50);

        // Assert
        Assert.Equal(10,_objective.CurrValue);
        Assert.True(completed); // completed
        Assert.True(_objective.IsCompleted); // Still completed
    }
}
