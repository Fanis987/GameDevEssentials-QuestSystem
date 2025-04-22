# GameDevEssentials-QuestSystem  
(3-5 min read)  
A collection of classes to create an in-game quest system.  
The package is compatible with **Godot 4.4 / .Net 8.0**  
**Suggested use**: Get the nuget package or simply copy-paste-addProjectRef

**Main Structure of the System**
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

// We start with some kind of manager class 
public partial class QuestManager : Node
{
    // Have a collection to store your quests
    public List<Quest> ActiveQuests { get; private set; } = new()

    public override void _Ready()
    {
        // Create a quest and add it to the list
        var quest = CreateSimpleQuest();
        ActiveQuests.Add( newQuest );
    }
    
    private Quest CreateSimpleQuest(){
        // Creating a quest follows the described structure
        // First we declare the objectives:
        // Args: Goal value, Id of the action that progresses the quest, asset affected
        // For example ActionId - 1: Gather 2: Hit  3:Talk 4: Kill
        // AssetId - OPTIONAL - Depends on the action type
        var gatherObjectiveSprites = new Objective(3, 1);
        var hitObjectiveSprites = new Objective(3, 2);

        // Then we must declare the stages
        // In this case there is only one stage
        string stageDescription = "Gather 3 plants and hit 3 targets";
        var stage = new QuestStageInclusive(stageDescription, gatherObjectiveSprites, hitObjectiveSprites);

        //Finally, create the quest object
        string questTitle = "Practicing the basics !";
        Quest newQuest = new Quest(1, questTitle, stage);
        return newQuest;
    }
    
    // Then we need a funuction that will be called from other nodes (or connected to events/signals)
    // Example : Player killed 3 wolf enemies
    // progressValue = 3 , taskId = 4 (based on above convention), assetId = 5 (for example)
    private void ProgressQuests( int progressValue,int taskId, int assetId = -1)
    {
        foreach(var quest in ActiveQuests)
        {
            quest.TryProgress( progressValue, taskId, assetId)
            if(quest.IsCompleted){
                //do sth / give rewards based on how your game works
                continue;
            }
            // otherwise maybe update some UI or log sth
            GD.Print(quest.Title);
        }
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
   Quest questFromJson = QuestParser.LoadFromJsonFile(jsonPath);
   return questFromJson;
}
```

