using QuestSystem;

namespace QuestSystemTests;

public class ObjectiveProgressDtoTests
{
    [Fact]
    public void CanCreateDto()
    {
        var dto1 = new ObjectiveProgressDto(2, 5, 3);
        var dto2 = new ObjectiveProgressDto(3, 6);
        
        Assert.Equal(2, dto1.TaskTypeId);
        Assert.Equal(5, dto1.ProgressValue);
        Assert.Equal(3, dto1.AssetId);
        Assert.Equal(3, dto2.TaskTypeId);
        Assert.Equal(6, dto2.ProgressValue);
        Assert.Equal(-1, dto2.AssetId);
    }
    
    [Fact]
    public void CanCheckEquality()
    {
        var dto1 = new ObjectiveProgressDto(2, 5, 3);
        var dto2 = new ObjectiveProgressDto(2, 5, 3);
        
        Assert.Equal(dto1, dto2);
    }
    
    [Fact]
    public void CanWriteToString()
    {
        var dto = new ObjectiveProgressDto(2, 5, 3);
        var expected = $"Objective Progress Dto- Progress: 5 on taskId:2 for asset:3";
        
        Assert.Equal(expected, dto.ToString());
    }
}