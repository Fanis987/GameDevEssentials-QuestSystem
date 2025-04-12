namespace QuestSystemTests.Entities;

public static class QuestJsons 
{
  
  public static string SmallQuestJson = @"
    {
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
    
  public static string mediumQuestJson = @"
    {
      ""Id"": 1,
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
              ""TaskTypeId"": 3,
              ""TargetAssetId"": 3
            }
          ]
        },
        {
          ""Description"": ""This is stage 2"",
          ""IsSelective"": false,
          ""Objectives"": []
        }
      ]
    }";

        
    
}