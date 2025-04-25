using QuestSystem.Parser.Dtos;

namespace QuestSystemTests.Parser;

public class DtoTests {
    
    private readonly ObjectiveDto _objectiveDto1 = new ObjectiveDto(10,1,1);
    private readonly ObjectiveDto _objectiveDto2 = new ObjectiveDto(5,1,2);
    
    private readonly QuestStageDto _stageSelectiveDto;
    private readonly QuestStageDto _stageInclusiveDto;
    
    private readonly QuestDto _questDto;

    public DtoTests()
    {
        var objDtoList = new List<ObjectiveDto>() { _objectiveDto1,_objectiveDto2 };
        
        _stageInclusiveDto = new QuestStageDto("inclusive descr",false,false, objDtoList);
        _stageSelectiveDto = new QuestStageDto("selective descr",false,true, objDtoList);

        var stageDtoList = new List<QuestStageDto>() { _stageInclusiveDto,_stageSelectiveDto };
        _questDto = new QuestDto(5, "quest title",true, stageDtoList);
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
    public void CanCreateStageDtos()
    {
        Assert.Equal("inclusive descr", _stageInclusiveDto.Description);
        Assert.False(_stageInclusiveDto.IsSelective);
        Assert.Equal(2, _stageInclusiveDto.Objectives.Count);
        Assert.Contains(_objectiveDto1, _stageInclusiveDto.Objectives);
        Assert.Contains(_objectiveDto2, _stageInclusiveDto.Objectives);

        Assert.Equal("selective descr", _stageSelectiveDto.Description);
        Assert.True(_stageSelectiveDto.IsSelective);
        Assert.Equal(2, _stageSelectiveDto.Objectives.Count);
        Assert.Contains(_objectiveDto1, _stageSelectiveDto.Objectives);
        Assert.Contains(_objectiveDto2, _stageSelectiveDto.Objectives);
    }

    [Fact]
    public void CanCreateQuestDto()
    {
        Assert.NotNull(_questDto);
        Assert.Equal(5, _questDto.Id);
        Assert.Equal("quest title", _questDto.Title);
        Assert.True(_questDto.IsMainQuest);

        // Assert stages
        Assert.NotNull(_questDto.Stages);
        Assert.Equal(2, _questDto.Stages.Count);

        var stage1 = _questDto.Stages[0];
        var stage2 = _questDto.Stages[1];

        // Check selective stage
        Assert.False(stage1.IsSelective);
        Assert.Equal("inclusive descr", stage1.Description);
        Assert.Equal(2, stage1.Objectives.Count);
        Assert.Contains(_objectiveDto1, stage1.Objectives);
        Assert.Contains(_objectiveDto2, stage1.Objectives);

        // Check inclusive stage
        Assert.True(stage2.IsSelective);
        Assert.Equal("selective descr", stage2.Description);
        Assert.Equal(2, stage2.Objectives.Count);
        Assert.Contains(_objectiveDto1, stage2.Objectives);
        Assert.Contains(_objectiveDto2, stage2.Objectives);
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
        Assert.Equal(2, quest.StagesLeft);
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