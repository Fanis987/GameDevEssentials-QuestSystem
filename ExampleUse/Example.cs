using QuestSystem;

namespace ExampleUse;

public class Example
{
    // Example Demonstration, assume:
    // Enemy ids: 1: fly 2:toad 3: wolf
    // NPC ids: 1: TownGuard 2: Salesman
    // Item ids: 1: beans , 2: mushrooms
    
    //Print function
    private static void PrintQuestStatus(Quest quest) {
        if (quest.IsCompleted)
        {
            Console.WriteLine($"Quest {quest.Id}: Stages Left: {quest.StagesLeft}");
            Console.WriteLine($"Quest {quest.Id}: Completed: {quest.IsCompleted}");
            return;
        }
        Console.WriteLine($"Quest {quest.Id}: Stages Left: {quest.StagesLeft}");
        if (quest.CurrentStage == null){ return;}
        Console.WriteLine($"Current Stage: {quest.CurrentStage.StageDescription}");
        Console.WriteLine($"Stage Progress: {quest.CurrentStage.StageProgress}");
        Console.WriteLine("Objective Progress:");
        foreach (var objectiveProgress in quest.CurrentStage.ObjectiveProgress) {
            Console.WriteLine(objectiveProgress);
        }
    }

    public static void Show1()
    {
        //Make a quest with 1 selective stage 
        var gatherObjectiveBeans  = new Objective(10, (int)ObjectiveType.Gather, 1);
        var gatherObjectiveShroom = new Objective(10, (int)ObjectiveType.Gather, 2);
        var stage31 = new QuestStageSelective("Find 10 beans OR 10 mushrooms", gatherObjectiveBeans,gatherObjectiveShroom);
        var quest3 = new Quest(3, "Gather Ingredients", stage31);
        
        // Define some progress actions
        var killedWolf     = new ObjectiveProgressDto((int)ObjectiveType.Kill,1,3);
        var gatheredShroom = new ObjectiveProgressDto((int)ObjectiveType.Gather,1,2);
        
        // Play the game
        Console.WriteLine($"Quest {quest3.Id}: {quest3.Title}, Stages Left: {quest3.StagesLeft}");

        Console.WriteLine("\nKilled a wolf");
        quest3.TryProgressQuest(killedWolf);
        PrintQuestStatus(quest3);

        Console.WriteLine("\nGathered a mushroom");
        quest3.TryProgressQuest(gatheredShroom);
        PrintQuestStatus(quest3);

        Console.WriteLine("\nGathered 8 mushrooms");
        for (int i = 0; i < 8; i++) { quest3.TryProgressQuest(gatheredShroom); }
        PrintQuestStatus(quest3);

        Console.WriteLine("\nGathered 2 mushrooms");
        for (int i = 0; i < 2; i++) { quest3.TryProgressQuest(gatheredShroom); }
        PrintQuestStatus(quest3);
    }
}