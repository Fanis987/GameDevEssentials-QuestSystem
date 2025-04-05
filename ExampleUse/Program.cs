// See https://aka.ms/new-console-template for more information

// Example Demonstration, assume:
// Enemy ids: 1: fly 2:toad 3: wolf
// NPC ids: 1: TownGuard 2: Salesman
// Item ids: 1: beans , 2: mushrooms

// Create some quests (Objective->Stage->Quest)
// Note: Single-stage quest can be declared in one line

using ExampleUse;
using QuestSystem;

var killObjectiveToad     = new Objective(5, (int)ObjectiveType.Kill, 2);
var quest1 = new Quest(1, "Defend from toads!", true, "Kill 5 toads", killObjectiveToad);

var killObjectiveWolf     = new Objective(3, (int)ObjectiveType.Kill, 3);
var talkObjectiveGuard    = new Objective(1, (int)ObjectiveType.Talk, 1);
var stage21 = new QuestStageInclusive("Kill 3 wolves", killObjectiveWolf);
var stage22 = new QuestStageInclusive("Talk to town guard", talkObjectiveGuard);
var quest2 = new Quest(2, "Trouble In Forest", stage21,stage22);

var gatherObjectiveBeans  = new Objective(10, (int)ObjectiveType.Gather, 1);
var gatherObjectiveShroom = new Objective(10, (int)ObjectiveType.Gather, 2);
var stage31 = new QuestStageSelective("Find 10 beans OR 10 mushrooms", gatherObjectiveBeans,gatherObjectiveShroom);
var quest3 = new Quest(3, "Gather Ingredients", stage31);

// Define some actions
var killedWolf     = new ObjectiveProgressDto((int)ObjectiveType.Kill,1,3);
var spokeToGuard   = new ObjectiveProgressDto((int)ObjectiveType.Talk,1,1);
var gatheredShroom = new ObjectiveProgressDto((int)ObjectiveType.Gather,1,2);

// Play the game
Console.WriteLine($"Quest {quest3.Id}: {quest3.Title}, Stages Left: {quest3.StagesLeft}");

Console.WriteLine("Killed a wolf");
quest3.TryProgressQuest(killedWolf);
Console.WriteLine($"Quest {quest3.Id}: Stages Left: {quest3.StagesLeft}");
Console.WriteLine($"Current Stage: {quest3.CurrentStage.StageDescription} - Objective Progress: {quest3.CurrentStage.StageProgress}");

Console.WriteLine("Gathered mushroom");
quest3.TryProgressQuest(gatheredShroom);
Console.WriteLine($"Quest {quest3.Id}: Stages Left: {quest3.StagesLeft}");
Console.WriteLine($"Current Stage: {quest3.CurrentStage.StageDescription} - Objective Progress: {quest3.CurrentStage.StageProgress}");