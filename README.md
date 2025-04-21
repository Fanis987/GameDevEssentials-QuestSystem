# GameDevEssentials-QuestSystem
A collection of classes to assist in creating a quest system in a game  
The package is compatible with **Godot 4.4 / .Net 8.0**

**Main Structure**
- A Quest is made from stages
- A stage is made up from objectives
- A stage can be inclusive (all objectives must be completed)
- Or selective (one of the objectives must be competed)

**Minimum Working Example in Godot 4.4**  
```csharp
using Godot;
//Assumes you have added the nuget package in your project
using QuestSystem;
using QuestSystem.Entities;
using System;


public partial class QuestManager : Node
{
    // Have a collection to store your quests
    public List<Quest> ActiveQuests { get; private set; } = new()

    public override void _Ready()
    {
        base._Ready();

        // Create the quest and add it to the list
        var quest = CreateSimpleQuest()
        ActiveQuests.Add( newQuest );
    }
    
    private Quest CreateSimpleQuest(){
        // Creating a quest follows the described structure
        // First we declare the objectives
        // Args: Goal value, Id of the action that progresses the quest
        // For example: 1: Gather something 2: Hit something etc (THE CONVENTION IS YOUR CHOICE)
        var gatherObjectiveSprites = new Objective(3, 1);
        var hitObjectiveSprites = new Objective(3, 2);

        // Then we must declare the stages
        // In this case there is only one stage
        string stageDescription = "Gather 3 plants and hit 3 targets";
        var stage = new QuestStageInclusive(stageDescription, gatherObjectiveSprites, hitObjectiveSprites);

        //Finally, create the quest and add it to the list
        string questTitle = "Practicing the basics !";
        Quest newQuest = new Quest(1, questTitle, stage)
        return newQuest
    }
}
```

**Quest details can be read from json too!**  
Json Example (somewhere in godot project directory):
```json
{
  "Id": 1,
  "Title": "First Quest",
  "Stages": [
    {
      "Description": "This is stage 1",
      "IsSelective": false,
      "Objectives": [
        {
          "GoalValue": 3,
          "TaskTypeId": 1,
          "TargetAssetId": 2
        },
        {
          "GoalValue": 3,
          "TaskTypeId": 2,
          "TargetAssetId": 7
        }
      ]
    }
  ]
}
```
Replace the function from minimal example with:
```csharp
private Quest CreateSimpleQuestViaJson(string jsonPath){
    // load from your path of choice
    // Can also pass serializer options as 2nd arg
   Quest questFromJson = QuestParser.LoadFromJsonFile(jsonPath)
   return questFromJson
}
```

