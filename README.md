# GameDevEssentials-QuestSystem  
(4-5 min read)  
A collection of classes to create an in-game quest system.  
The package is compatible with **Godot 4.4 / .Net 8.0**  
**Suggested use**: Get the nuget package or simply copy-paste-addProjectRef

**Main Structure of the System**
- A Quest is made from stages
- A stage is made up from objectives
- A stage can be inclusive (all objectives must be completed)
- Or selective (one of the objectives must be competed)

**Minimum Working Example in Godot 4.4**  
**METHOD 1 : Create a quests via code**  
```csharp
using Godot;
//Assumes you have added the nuget package in your project
using QuestSystem;
using QuestSystem.Entities;
using System;

// We start with some kind of manager class 
public partial class QuestManager : Node
{
    // Have a collection to store your in-game quests
    public List<Quest> ActiveQuests { get; private set; } = new()

    public override void _Ready()    {
        CreateSimpleQuest();
    }
    
    // Creating a quest follows the described structure
    private void CreateSimpleQuest(){
        // Assume in-game action/ objective type Ids: 1: Gather 2: Hit  3:Talk  4: Kill etc.
        // Assume in-game item ids: 1: sword 2: red flower  3: grass etc.
        // Assume in-game enemy ids: 1: Toad 2: Rabbit 3:Wolf 4: Turtle etc.
        
        // First we declare the objectives:
        // Args: Goal value, Id of the action that progresses the quest, asset affected
        // 3 gathering (id=1) of red flowers (assetId=2)
        var gatherFlowerObjective = new Objective(3, 1, 2);
        // 5 hits (id=2) on toads (assetId=1)
        var hitToadObjective      = new Objective(5, 2, 1);
        var objectiveList = new List<Objective>(){gatherFlowerObjective,hitToadObjective};

        // Then we must declare the stages
        // In this case there is only one stage
        string stageDescription = "Gather 3 red flowers AND hit 5 toads";
        // Normal Stages: ALL objectives must be completed
        // Selective Stages: ANY of the objectives must be completed
        bool isSelective = false;
        var stage = new QuestStage(stageDescription,isSelective, objectiveList);

        //Finally, create the quest object and add it to the list
        string questTitle = "Practicing the basics !";
        Quest newQuest = new Quest(1, questTitle, stage);
        ActiveQuests.Add( newQuest );
    }
    
    // Then we need a progress function that will be called from other nodes (or connected to events/signals)
    // Example Scenario: The player killed 3 wolf enemies, based on above convention:
    // progressValue = 3 , taskId = 4 (kill action), assetId = 3 (wolf)
    private void ProgressQuests( int progressValue,int taskId, int assetId = -1)
    {
        var activeQuestsCopy = new List<Quest>(ActiveQuests) //avoid mid-loop deletion issues
        foreach(var quest in activeQuestsCopy)
        {
            quest.TryProgress( progressValue, taskId, assetId);
            if(quest.IsCompleted){
                // Do sth like giving rewards based on how your game works
                ActiveQuests.Remove(quest);
                continue;
            }
            // Otherwise maybe update some UI or log sth with the quest progress
            // You can use exposed properties of the quest object for this:
            GD.Print(quest.Title);
        }
    }    
}
```
**METHOD 2 : Read Quest details from a json file!**  
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
Note: can also have json with array of quests

Replace the function from minimal example with:
```csharp
private List<Quest> LoadQuestsFromJson(string jsonPath){
    // Load from your path of choice
    // Can also pass serializer options as 2nd arg
    MultiParseResult parseResult = QuestParser.LoadFromJsonFile(jsonPath);
    
    // Here you can access your parsed quests
    List<Quest> parsedQuestList = parseResult.Quests;
    
    // Check for parsing errors (useful for many quests in a json)
    var errorsList = parseResult.ErrorMessages;
    if(errorsList.Count != 0){
        foreach(var error in errorsList) GD.PrintErr(error)
    }
    
    return parsedQuestList;
}
```

To improve readability during use you can also declare enums, e.g.:
```csharp
// Assume in-game action/ objective type Ids: 1: Gather 2: Hit  3:Talk  4: Kill etc.
public enum TaskType
{
    Gather = 1,
    Hit= 2,
    Talk= 3,
    Kill= 4
}
```
And create overrides like:
```csharp
private void ProgressQuests( int progressValue,TaskType taskType, int assetId = -1){
    ProgressQuests( int progressValue,(int) taskType, int assetId = -1)
    }
```

