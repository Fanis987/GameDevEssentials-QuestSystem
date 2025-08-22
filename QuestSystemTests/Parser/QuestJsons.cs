namespace QuestSystemTests.Parser;

public static class QuestJsons
{

  public static readonly string EmptyJson = "";
  public static readonly string NumberJson = "234";
  public static readonly string SpecialCharJson = "'34";
  
  public static readonly string SmallQuestJson = @"{
  ""Id"": 1,
  ""Title"": ""First Quest"",
  ""NextQuestId"": 2,
  ""IsMainQuest"": true,
  ""Stages"": [
    {
      ""Id"": 1,
      ""Description"": ""This is stage 1"",
      ""IsCompleted"": false,
      ""PathDtos"": [
        {
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
}";
  
  public static readonly string StringAtIntPosJson = @"{
  ""Id"": 1,
  ""Title"": ""First Quest"",
  ""NextQuestId"": 2,
  ""IsMainQuest"": true,
  ""Stages"": [
    {
      ""Id"": 1,
      ""Description"": ""This is stage 1"",
      ""IsCompleted"": false,
      ""PathDtos"": [
        {
          ""IsSelective"": false,
          ""Objectives"": [
            {
              ""GoalValue"": ""Bob"",
              ""TaskTypeId"": 3,
              ""TargetAssetId"": 2
            }
          ]
        }
      ]
    }
  ]
}";
  
  public static readonly string NumAtBoolPosJson = @"{
  ""Id"": 1,
  ""Title"": ""First Quest"",
  ""NextQuestId"": 2,
  ""IsMainQuest"": 8,
  ""Stages"": [
    {
      ""Id"": 1,
      ""Description"": ""This is stage 1"",
      ""IsCompleted"": false,
      ""PathDtos"": [
        {
          ""IsSelective"": false,
          ""Objectives"": [
            {
              ""GoalValue"": ""Bob"",
              ""TaskTypeId"": 3,
              ""TargetAssetId"": 2
            }
          ]
        }
      ]
    }
  ]
}";
  
  public static readonly string QuestNoIdJson = @"{
      ""Title"": ""First Quest"",
      ""Stages"": [
        {
          ""Id"": 1,
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
        ""Id"": 1,
        ""Description"": ""This is stage 1"",
        ""IsCompleted"": false,
        ""PathDtos"": [
          {
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
  }";
  
  
  public static readonly string QuestInvalidId = @"{
    ""Id"": bob,
    ""Title"": ""First Quest"",
    ""Stages"": [
      {
        ""Id"": 1,
        ""Description"": ""This is stage 1"",
        ""IsCompleted"": false,
        ""PathDtos"": [
          {
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
  }";
  
  public static readonly string QuestListInvalidId = @"[
  {
    ""Id"": bob,
    ""Title"": ""First Quest"",
    ""Stages"": [
      {
        ""Id"": 1,
        ""Description"": ""This is stage 1"",
        ""IsCompleted"": false,
        ""PathDtos"": [
          {
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
  },
  {
    ""Id"": bob2,
    ""Title"": ""Second Quest"",
    ""Stages"": [
      {
        ""Id"": 1,
        ""Description"": ""This is stage 1-2"",
        ""IsCompleted"": false,
        ""PathDtos"": [
          {
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
        ""Id"": 1,
        ""Description"": ""This is stage 1-2"",
        ""IsCompleted"": false,
        ""PathDtos"": [
          {
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
    ]
  }
]";
  
  public static readonly string QuestNoGoal = @"{
  ""Id"": 3,
  ""Title"": ""First Quest"",
  ""Stages"": [
    {
      ""Id"": 1,
      ""Description"": ""This is stage 1"",
      ""IsCompleted"": false,
      ""PathDtos"": [
        {
          ""IsSelective"": false,
          ""Objectives"": [
            {
              ""TaskTypeId"": 3,
              ""TargetAssetId"": 2
            }
          ]
        }
      ]
    }
  ]
}";
  
  public static readonly string QuestNegativeGoal = @"{
  ""Id"": 3,
  ""Title"": ""First Quest"",
  ""Stages"": [
    {
      ""Id"": 1,
      ""Description"": ""This is stage 1"",
      ""IsCompleted"": false,
      ""PathDtos"": [
        {
          ""IsSelective"": false,
          ""Objectives"": [
            {
              ""GoalValue"": -1,
              ""TaskTypeId"": 3,
              ""TargetAssetId"": 2
            }
          ]
        }
      ]
    }
  ]
}";
  
  public static readonly string QuestNegativeTAskId = @"{
  ""Id"": 3,
  ""Title"": ""First Quest"",
  ""Stages"": [
    {
      ""Id"": 1,
      ""Description"": ""This is stage 1"",
      ""IsCompleted"": false,
      ""PathDtos"": [
        {
          ""IsSelective"": false,
          ""Objectives"": [
            {
              ""GoalValue"": 1,
              ""TaskTypeId"": -3,
              ""TargetAssetId"": 2
            }
          ]
        }
      ]
    }
  ]
}";
  
  public static readonly string QuestLargeTaskId = @"{
  ""Id"": 3,
  ""Title"": ""First Quest"",
  ""Stages"": [
    {
      ""Id"": 1,
      ""Description"": ""This is stage 1"",
      ""IsCompleted"": false,
      ""PathDtos"": [
        {
          ""IsSelective"": false,
          ""Objectives"": [
            {
              ""GoalValue"": 1,
              ""TaskTypeId"": 10000,
              ""TargetAssetId"": 2
            }
          ]
        }
      ]
    }
  ]
}";
  
  
  public static readonly string QuestLargeAssetId = @"{
  ""Id"": 3,
  ""Title"": ""First Quest"",
  ""Stages"": [
    {
      ""Id"": 1,
      ""Description"": ""This is stage 1"",
      ""IsCompleted"": false,
      ""PathDtos"": [
        {
          ""IsSelective"": false,
          ""Objectives"": [
            {
              ""GoalValue"": 1,
              ""TaskTypeId"": 2,
              ""TargetAssetId"": 10000
            }
          ]
        }
      ]
    }
  ]
}";
  
  public static readonly string QuestLargeGoalValue = @"{
  ""Id"": 3,
  ""Title"": ""First Quest"",
  ""Stages"": [
    {
      ""Id"": 1,
      ""Description"": ""This is stage 1"",
      ""IsCompleted"": false,
      ""PathDtos"": [
        {
          ""IsSelective"": false,
          ""Objectives"": [
            {
              ""GoalValue"": 1000000,
              ""TaskTypeId"": 10,
              ""TargetAssetId"": 2
            }
          ]
        }
      ]
    }
  ]
}";
  
  public static readonly string QuestLongTitleJson = @"{
  ""Id"": 3,
  ""Title"": ""First Quest: The Ancient Prophecy of the Fallen Ancient Stars and the Eternal Guardians Who Wander the Forgotten Lands Seeking Redemption in the Shadow of the Moonlit Citadel of Everlasting Twilight and Beyond The Abyss"",
  ""Stages"": [
    {
      ""Id"": 1,
      ""Description"": ""This is stage 1"",
      ""IsCompleted"": false,
      ""PathDtos"": [
        {
          ""IsSelective"": false,
          ""Objectives"": [
            {
              ""GoalValue"": 1,
              ""TaskTypeId"": -3,
              ""TargetAssetId"": 2
            }
          ]
        }
      ]
    }
  ]
}";
  
  public static readonly string QuestLongDescriptionJson = @"{
  ""Id"": 3,
  ""Title"": ""First Quest: The Ancient Prophecy"",
  ""Stages"": [
    {
      ""Id"": 1,
      ""Description"": ""Long ago, in the darkened past, when the world was still young and the first empires carved their names into the stone of history, a prophecy was written upon the walls of the Moonlit Citadel. It spoke of fallen stars, guardians bound by honor, a veil around time and an endless struggle between light and shadow that would echo through countless generations. Many dismissed the tale as myth, yet fragments of the prophecy have resurfaced across ruined temples, shattered scrolls, and whispered songs carried by nomads of the northern wastes. The time of fulfillment may be near: celestial signs have returned, creatures once thought extinct roam again, and restless powers stir beneath the mountains. You must journey into lands abandoned for centuries, face dangers that defy mortal understanding, creatures of myth and uncover the truth hidden within the prophecy. Beware, for even allies may fall into darkness when tempted by ancient power, and the path forward may demand sacrifices beyond imagination.Long ago, in the darkened past, when the world was still young and the first empires carved their names into the stone of history, a prophecy was written upon the walls of the Moonlit Citadel. It spoke of fallen stars, guardians bound by honor, a veil around time and an endless struggle between light and shadow that would echo through countless generations. Many dismissed the tale as myth, yet fragments of the prophecy have resurfaced across ruined temples, shattered scrolls, and whispered songs carried by nomads of the northern wastes. The time of fulfillment may be near: celestial signs have returned, creatures once thought extinct roam again, and restless powers stir beneath the mountains. You must journey into lands abandoned for centuries, face dangers that defy mortal understanding, creatures of myth and uncover the truth hidden within the prophecy. Beware, for even allies may fall into darkness when tempted by ancient power, and the path forward may demand sacrifices beyond imagination."",
      ""IsCompleted"": false,
      ""PathDtos"": [
        {
          ""IsSelective"": false,
          ""Objectives"": [
            {
              ""GoalValue"": 1,
              ""TaskTypeId"": -3,
              ""TargetAssetId"": 2
            }
          ]
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
  
  public static readonly string StageWithNoPathDtosJson = @"{
    ""Id"": 3,
    ""Title"": ""Quest with No PathDtos"",
    ""IsMainQuest"": true,
    ""NextQuestId"": 4,
    ""Stages"": [
      {
        ""Id"": 1,
        ""Description"": ""This stage has no PathDtos"",
        ""IsCompleted"": false,
        ""PathDtos"": []
      }
    ]
  }"; 
  
  public static readonly string StageWithNoDescriptionJson = @"{
    ""Id"": 3,
    ""Title"": ""Quest with No PathDtos"",
    ""IsMainQuest"": true,
    ""NextQuestId"": 4,
    ""Stages"": [
      {
        ""Id"": 1,
        ""IsCompleted"": false,
        ""PathDtos"": [
        {
          ""IsSelective"": false,
          ""Objectives"": [
            {
              ""GoalValue"": 1,
              ""TaskTypeId"": 3,
              ""TargetAssetId"": 2
            }
          ]
        }
      ]
      }
    ]
  }";
  
  public static readonly string CompletedStageJson = @"{
    ""Id"": 3,
    ""Title"": ""Quest 3"",
    ""Stages"": [
      {
        ""Id"": 1,
        ""Description"": ""This is stage 1"",
        ""IsCompleted"": true,
        ""PathDtos"": [
          {
            ""IsSelective"": false,
            ""Objectives"": [
              {
                ""Description"": ""This should not exist""
              }
            ]
          }
        ]
      }
    ]
  }";
  
  public static readonly string StageWithNegativeIdJson = @"{
    ""Id"": 1,
    ""Title"": ""Quest with Invalid Stage"",
    ""IsMainQuest"": true,
    ""NextQuestId"": 2,
    ""Stages"": [
      {
        ""Id"": -1,
        ""Description"": ""This stage has an invalid ID"",
        ""IsCompleted"": false,
        ""PathDtos"": [
          {
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
  }";
  
  public static readonly string StageWithNoObjectivesJson = @"{
    ""Id"": 4,
    ""Title"": ""Quest 4"",
    ""Stages"": [
      {
        ""Id"": 1,
        ""Description"": ""This is stage 1"",
        ""IsCompleted"": false,
        ""PathDtos"": [
          {
            ""IsSelective"": false,
            ""Objectives"": []
          }
        ]
      }
    ]
  }";
  
  public static readonly string MediumQuestJson = @"{
  ""Id"": 5,
  ""Title"": ""The example quest"",
  ""Stages"": [
    {
      ""Id"": 1,
      ""Description"": ""This is stage 1"",
      ""IsCompleted"": false,
      ""PathDtos"": [
        {
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
        }
      ]
    },
    {
      ""Id"": 2,
      ""Description"": ""This is stage 2"",
      ""IsCompleted"": false,
      ""PathDtos"": [
        {
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
    }
  ]
}";

  
  public static readonly string MultiQuestJson = @"[
  {
    ""Id"": 6,
    ""Title"": ""First Quest In Multi"",
    ""IsMainQuest"": true,
    ""NextQuestId"": 7,
    ""Stages"": [
      {
        ""Id"": 1,
        ""Description"": ""This is stage 1"",
        ""IsCompleted"": false,
        ""PathDtos"": [
          {
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
  },
  {
    ""Id"": 7,
    ""Title"": ""Second Quest In Multi"",
    ""IsMainQuest"": true,
    ""Stages"": [
      {
        ""Id"": 1,
        ""Description"": ""This is stage 1"",
        ""IsCompleted"": false,
        ""PathDtos"": [
          {
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
  }
]";

}