namespace QuestSystemTests.Parser;

public static class QuestJsons
{

  public static readonly string EmptyJson = "";
  public static readonly string NumberJson = "234";
  public static readonly string SpecialCharJson = "'34";
  
  public static readonly string SmallQuestJson = @"{
      ""Id"": 1,
      ""Title"": ""First Quest"",
      ""Stages"": [
        {
          ""Description"": ""This is stage 1"",
          ""IsSelective"": false,
          ""Objectives"": [
            {
              ""GoalValue"": 5,
              ""TaskTypeId"": 3,
              ""TargetAssetId"": 2
            }
          ]
        }
      ]
    }";
  
  public static readonly string NoStagesJson = @"{
      ""Id"": 2,
      ""Stages"": []
    }";
  
  public static readonly string CompletedStageJson = @"{
      ""Id"": 3,
      ""Stages"": [
        {
          ""IsCompleted"": true,
          ""Objectives"": [
            {
              ""Description"": ""This should not exist""
            }
          ]
        }
      ]
    }";
  
  public static readonly string StageWithNoObjectivesJson = @"{
      ""Id"": 4,
      ""Stages"": [
        {
          ""IsCompleted"": false,
          ""Objectives"": []
        }
      ]
    }";

  public static readonly string MediumQuestJson = @"{
      ""Id"": 5,
      ""Title"": ""The example quest"",
      ""Stages"": [
        {
          ""Description"": ""This is stage 1"",
          ""IsSelective"": false,
          ""Objectives"": [
            {
              ""GoalValue"": 5,
              ""TaskTypeId"": 3,
              ""TargetAssetId"": 2
            },
            {
              ""GoalValue"": 10,
              ""TaskTypeId"": 3
            }
          ]
        },
        {
          ""Description"": ""This is stage 2"",
          ""IsSelective"": false,
          ""Objectives"": [
            {
              ""GoalValue"": 1,
              ""TaskTypeId"": 1,
              ""TargetAssetId"": 5
            }
          ]
        }
      ]
    }";
  
  public static readonly string MultiQuestJson = @"[
    {
      ""Id"": 6,
      ""Title"": ""First Quest In Multi"",
      ""Stages"": [
        {
          ""Description"": ""This is stage 1"",
          ""IsSelective"": false,
          ""Objectives"": [
            {
              ""GoalValue"": 5,
              ""TaskTypeId"": 3,
              ""TargetAssetId"": 2
            }
          ]
        }
      ]
    },

    {
      ""Id"": 7,
      ""Title"": ""Second Quest In Multi"",
      ""Stages"": [
        {
          ""Description"": ""This is stage 1"",
          ""IsSelective"": false,
          ""Objectives"": [
            {
              ""GoalValue"": 5,
              ""TaskTypeId"": 3,
              ""TargetAssetId"": 2
            }
          ]
        }
      ]
    }
  ]
";
}