# GameDevEssentials-QuestSystem  
(4-5 min read)  
A collection of classes to create an in-game quest system.  
The package is compatible with **Godot 4.4 / .Net 8.0**  
**Suggested use**: Get the nuget package or simply copy-paste-addProjectRef

**Main Structure of the System**  
Key entities: Objective, StagePath, QuestStage, Quest
- A Quest is made from one or more stages.
- Each stage is made up from one or more stage paths.
- Each stage path indicates the next stage unlocked upon its completion. (aka quest branching)
- A stage path consists of one or more objectives.
- A stage path can be normal. (all objectives must be completed)
- Or selective (only one objective must be competed)

**Assuming a 'QuestManager' Node, we can create quests in 2 ways:**  
**METHOD 1 : Create a quests via code:**  
```csharp
// Assume in-game action/ objective type Ids: 1: Gather 2: Hit  3:Talk  4: Kill etc.
// Assume in-game item ids: 1: sword 2: red flower  3: grass etc.
// Assume in-game enemy ids: 1: Toad 2: Rabbit 3:Wolf 4: Turtle etc.

// A very simple quest: 2 objectives - 1 stage path - 1 stage
private Quest CreateSimpleQuest(){
    
    // First we declare the objectives:
    // Args: Goal value, Id of the action that progresses the quest, asset affected
    // 3 gathering (id=1) of red flowers (assetId=2)
    var gatherFlowerObjective = new Objective(3, 1, 2);
    // 5 hits (id=2) on toads (assetId=1)
    var hitToadObjective      = new Objective(5, 2, 1);

    // Then we combine them in a stage path
    // Normal Path: ALL objectives must be completed to complete the path.
    // Selective Path: ANY of the objectives must be completed to complete the path.
    bool isSelective = false;
    // Each path indicates the stage unlocked, upon its completion
    // If completeing this path ends the quest, set to -1
    int nextStageId = -1 // in this case there is no next stage
    StagePath path = new StagePath(isSelective, nextStageId, gatherFlowerObjective,hitToadObjective)
    
    //We combine the paths to a stage
    int stageId = 1; //Should be unique per quest
    string stageDescription = "Gather 3 red flowers AND hit 5 toads";
    QuestStage stage = new QuestStage(stageId,stageDescription, path);

    //Finally, create the quest object and add it to the list
    int questId = 1; //Should be unique
    string questTitle = "Practicing the basics !";
    bool isMainQuest = true;// Optional: Common need to discriminate main and optional quests
    bool nextQuestId = 5;  //  Optional: For quest-chain support
    Quest newQuest = new Quest(questId, questTitle,isMainQuest,nextQuestId, stage);

    }
}
```
**METHOD 2 : Parse Quest details from a json file!**
```json
[
  {
    "Id": 1,
    "Title": "First Quest",
    "Stages": [
      {
        "Id": 1,
        "Description": "This is stage 1 of first quest",
        "PathDtos": [
          {
            "IsSelective": false,
            "NextStageId": 2,
            "Objectives": [
              {
                "GoalValue": 5,
                "TaskTypeId": 3,
                "TargetAssetId": 2
              },
              {
                "GoalValue": 7,
                "TaskTypeId": 2
              }
            ]
          }
        ]
      },
      {
        "Id": 2,
        "Description": "This is stage 2 of first quest",
        "PathDtos": [
          {
            "IsSelective": true,
            "NextStageId": -1,
            "Objectives": [
              {
                "GoalValue": 5,
                "TaskTypeId": 2,
                "TargetAssetId": 8
              },
              {
                "GoalValue": 6,
                "TaskTypeId": 1
              }
            ]
          }
        ]
      }
    ]
  },
  {
    "Id": 2,
    "Title": "Second Quest",
    "Stages": [
      {
        "Id": 1,
        "Description": "This is stage 1 of the second quest",
        "IsCompleted": false,
        "PathDtos": [
          {
            "IsSelective": false,
            "NextStageId": -1,
            "Objectives": [
              {
                "GoalValue": 5,
                "TaskTypeId": 3,
                "TargetAssetId": 2
              }
            ]
          }
        ]
      }
    ]
  }
]
```
Load the multiple quests in json as follows:
```csharp
private List<Quest> LoadQuestsFromJson(string jsonPath){
    // Load from your path of choice
    // Can also pass serializer options as 2nd arg
    MultiParseResult parseResult = QuestParser.LoadFromJsonFile(jsonPath);
    
    // Here you can access your parsed quests
    List<Quest> parsedQuestList = parseResult.Quests;
    
    // Check for parsing errors (useful for many quests present in a single json)
    var errorsList = parseResult.ErrorMessages;
    if(errorsList.Count != 0){
        foreach(var error in errorsList) GD.PrintErr(error);
    }
    return parsedQuestList;
}
```

Next we need a function to progress the active quests:
```csharp
// Example Scenario: The player killed 3 wolf enemies, based on above convention:
// progressValue = 3 , taskId = 4 (kill action), assetId = 3 (wolf)
private void ProgressQuests( int progressValue,int taskId, int assetId = -1)
{
    GD.Print($"\nReceived progress {progressValue} for taskId:{taskId}, for asset: {assetId}");
    
    // Asumming you store your active quests in a list:
    var activeQuestsCopy = new List<Quest>(ActiveQuests); //avoid mid-loop deletion issues
    foreach(var quest in activeQuestsCopy){
        // progress using the in-package progress function
        quest.TryProgressQuest(progressValue, taskId, assetId);
        
        // Check for completion and rewards
        if(quest.IsCompleted){
            GD.Print($"\nQuest {quest.Id} COMPLETED");
            // Here do sth like giving rewards based on how your game works
            ActiveQuests.Remove(quest);
        
            // Can also use logic to start next quest in chain (if it exists)
            if(quest.NextQuestId == 0) continue;
            // Asumming you store all game quests in a another list or class:
            var nextQuest = AllQuests.FirstOrDefault( q => q.Id == quest.NextQuestId );
            if(nextQuest== null) continue;
            ActiveQuests.Add(nextQuest);
            GD.Print($"Added Next quest in chain, with id: {nextQuest.Id}");
            continue;
        }
        // Otherwise maybe update some UI or log sth with the quest progress
    }
}
```
**TIP**  
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

