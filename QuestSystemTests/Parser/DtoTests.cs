using QuestSystem.Entities;
using QuestSystem.Parser.Dtos;

namespace QuestSystemTests.Parser;

public class DtoTests {
    
    private readonly ObjectiveDto _objectiveDto1 = new ObjectiveDto(10,1,1);
    private readonly ObjectiveDto _objectiveDto2 = new ObjectiveDto(5,1,2);
    
    private readonly StagePathDto _stagePathDtoSelective;
    private readonly StagePathDto _stagePathDtoNormal;
    
    private readonly QuestStageDto _stageSelectiveDto;
    private readonly QuestStageDto _stageInclusiveDto;
    
    private readonly QuestDto _questDto;

    public DtoTests()
    {
        var objDtoList = new List<ObjectiveDto>() { _objectiveDto1,_objectiveDto2 };
        
        _stagePathDtoNormal    = new StagePathDto(false, objDtoList);
        _stagePathDtoSelective = new StagePathDto(true , objDtoList);
        
        _stageInclusiveDto = new QuestStageDto(1,"inclusive descr",false,new(){_stagePathDtoNormal});
        _stageSelectiveDto = new QuestStageDto(2,"selective descr",false,new(){_stagePathDtoSelective});

        var stageDtoList = new List<QuestStageDto>() { _stageInclusiveDto,_stageSelectiveDto };
        _questDto = new QuestDto(5, "quest title",true,11, stageDtoList);
    }
    
    [Fact]
    public void CanCreateObjectiveDto()
    {
        Assert.Equal(10, _objectiveDto1.GoalValue);
        Assert.Equal(1, _objectiveDto1.TaskTypeId);
        Assert.Equal(1, _objectiveDto1.TargetAssetId);

        Assert.Equal(5, _objectiveDto2.GoalValue);
        Assert.Equal(1, _objectiveDto2.TaskTypeId);
        Assert.Equal(2, _objectiveDto2.TargetAssetId);
    }
    
    [Fact]
    public void CanCreateStagePathDtos()
    {
        Assert.True(_stagePathDtoSelective.IsSelective);
        Assert.Equal(2, _stagePathDtoSelective.Objectives.Count);
        Assert.Contains(_objectiveDto1, _stagePathDtoSelective.Objectives);
        Assert.Contains(_objectiveDto2, _stagePathDtoSelective.Objectives);
        
        Assert.False(_stagePathDtoNormal.IsSelective);
        Assert.Equal(2, _stagePathDtoNormal.Objectives.Count);
        Assert.Contains(_objectiveDto1, _stagePathDtoNormal.Objectives);
        Assert.Contains(_objectiveDto2, _stagePathDtoNormal.Objectives);
    }

    [Fact]
    public void CanCreateStageDtos() {
        Assert.Equal("inclusive descr", _stageInclusiveDto.Description);
        Assert.Single(_stageInclusiveDto.PathDtos);
        Assert.Contains(_stagePathDtoNormal, _stageInclusiveDto.PathDtos);

        Assert.Equal("selective descr", _stageSelectiveDto.Description);
        Assert.Single(_stageSelectiveDto.PathDtos);
    }

    [Fact]
    public void CanCreateQuestDto()
    {
        Assert.NotNull(_questDto);
        Assert.Equal(5, _questDto.Id);
        Assert.Equal("quest title", _questDto.Title);
        Assert.Equal(11, _questDto.NextQuestId);
        Assert.True(_questDto.IsMainQuest);

        // Assert stages
        Assert.NotNull(_questDto.Stages);
        Assert.Equal(2, _questDto.Stages.Count);

        var stage1 = _questDto.Stages[0];
        var stage2 = _questDto.Stages[1];

        // Check selective stage
        Assert.Equal("inclusive descr", stage1.Description);
        Assert.Single(stage1.PathDtos);

        // Check inclusive stage
        Assert.Equal("selective descr", stage2.Description);
        Assert.Single(stage2.PathDtos);
    }

    [Fact]
    public void CanConvertObjectiveDto()
    {
        var objective = _objectiveDto1.ToObjective();
        
        Assert.Equal(10, objective.GoalValue);
        Assert.Equal(1, objective.TaskTypeId);
        Assert.Equal(1, objective.TargetAssetId);
    }
    
    [Fact]
    public void CanConvertStagePathDtos()
    {
        var stagePathInclusive = _stagePathDtoNormal.ToStagePath();
        var stagePathSelective = _stagePathDtoSelective.ToStagePath();
        
        Assert.False(stagePathInclusive.IsSelective);
        Assert.Equal(0, stagePathInclusive.CompletedObjectiveCount);
        Assert.False(stagePathInclusive.IsCompleted);

        Assert.True(stagePathSelective.IsSelective);
        Assert.Equal(0, stagePathSelective.CompletedObjectiveCount);
        Assert.False(stagePathSelective.IsCompleted);
    }
    
    [Fact]
    public void CanConvertStageDtos()
    {
        var stageInclusive = _stageInclusiveDto.ToQuestStage();
        var stageSelective = _stageSelectiveDto.ToQuestStage();
        
        Assert.Equal("inclusive descr", stageInclusive.StageDescription);
        Assert.False(stageInclusive.IsCompleted);

        Assert.Equal("selective descr", stageSelective.StageDescription);
        Assert.False(stageSelective.IsCompleted);
    }
    
    [Fact]
    public void CanConvertQuestDto()
    {
        var quest = _questDto.ToQuest();
        
        Assert.NotNull(quest);
        Assert.Equal(5, quest.Id);
        Assert.Equal("quest title", quest.Title);
        Assert.False(quest.IsCompleted);
        
        // Assert stages
        Assert.NotNull(_questDto.Stages);
        Assert.Equal(2, _questDto.Stages.Count);

        var stage = quest.CurrentStage;
        Assert.NotNull(stage);

        // Check selective stage
        Assert.False(stage.IsCompleted);
        Assert.Equal("inclusive descr", stage.StageDescription);
 }
    
    
}