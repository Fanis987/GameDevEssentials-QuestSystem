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
  
  public static readonly string QuestNoIdJson = @"{
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
  
  public static readonly string QuestNoTitleJson = @"{
      ""Id"": 2,
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
  
  
  public static readonly string QuestInvalidId = @"{
      ""Id"": bob,
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
  
  public static readonly string QuestListInvalidId = @"[
      {
        ""Id"": bob,
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
      },
      {
        ""Id"": bob2,
        ""Title"": ""Second Quest"",
        ""Stages"": [
          {
            ""Description"": ""This is stage 1-2"",
            ""IsSelective"": false,
            ""Objectives"": [
              {
                ""GoalValue"": 56,
                ""TaskTypeId"": 37,
                ""TargetAssetId"": 28
              }
            ]
          }
        ]
      }
    ]";
  
  public static readonly string QuestListNoStages = @"[
      {
        ""Id"": 1,
        ""Title"": ""First Quest"",
        ""Stages"": []
      },
      {
        ""Id"": 2,
        ""Title"": ""Second Quest"",
        ""Stages"": [
          {
            ""Description"": ""This is stage 1-2"",
            ""IsSelective"": false,
            ""Objectives"": [
              {
                ""GoalValue"": 56,
                ""TaskTypeId"": 37,
                ""TargetAssetId"": 28
              }
            ]
          }
        ]
      }
    ]";
  
  public static readonly string QuestNoGoal = @"{
      ""Id"": 3,
      ""Title"": ""First Quest"",
      ""Stages"": [
        {
          ""Description"": ""This is stage 1"",
          ""IsSelective"": false,
          ""Objectives"": [
            {
              ""TaskTypeId"": 3,
              ""TargetAssetId"": 2
            }
          ]
        }
      ]
    }";
  
  public static readonly string NoStagesJson = @"{
      ""Id"": 2,
      ""Title"": ""Quest 2"",
      ""Stages"": []
    }";
  
  public static readonly string CompletedStageJson = @"{
      ""Id"": 3,
      ""Title"": ""Quest 3"",
      ""Stages"": [
        {
          ""Description"": ""This is stage 1"",          
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
      ""Title"": ""Quest 4"",
      ""Stages"": [
        {
          ""Description"": ""This is stage 1"",
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