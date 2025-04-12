using ExampleUse;
using QuestSystem.Parser;

// Code-based Quest Generation
// var quest = Example.CreateMultiStageQuest();
// Example.CompleteMultiStageQuest(quest);

// Json-based Quest Generation
string jsonString = File.ReadAllText("Example.json");
var questList = QuestParser.LoadFromJson(jsonString);
Console.WriteLine($"Found {questList.Count} quests in json file");
Console.WriteLine($"First quest id:{questList[0].Id} title:{questList[0].Title}");
Console.WriteLine($"First quest id:{questList[1].Id} title:{questList[1].Title}");