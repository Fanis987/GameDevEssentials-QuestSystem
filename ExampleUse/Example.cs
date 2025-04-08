using QuestSystem;

namespace ExampleUse;

public static class Example
{
    // Example Demonstration, assume:
    // Enemy ids: 1: fly 2:toad 3: wolf
    // NPC ids: 1: TownGuard 2: Salesman
    // Item ids: 1: beans , 2: mushrooms

    /// <summary>
    /// A basic example of creating a quest with a single stage, the normal way
    /// The quest includes gathering 10 Beans and 10 mushrooms
    /// </summary>
    public static Quest CreateSingleStageQuest() {
        // First we define the objectives of the stage
        //Note: Defining enums instead of passing pure integers everywhere makes the code more readable.
        var gatherObjectiveBeans  = new Objective(10, (int)ObjectiveType.Gather, 1);
        var gatherObjectiveShroom = new Objective(10, (int)ObjectiveType.Gather, 2);
        
        // Then we define the stage that includes these objectives
        var stageDescription = "gathering 10 Beans and 10 mushrooms";
        var stage = new QuestStageInclusive(stageDescription,gatherObjectiveBeans, gatherObjectiveShroom);
        
        // Finally we create the quest
        var questTitle = "Gather Ingredients";
        var quest = new Quest(1, questTitle , stage);

        return quest;
    }
    
    /// <summary>
    /// A basic example of creating a quest with a single stage, the fast way
    /// The quest includes gathering 10 Beans and 10 mushrooms
    /// </summary>
    public static Quest CreateSingleStageQuestShort() {
        // First we define the objectives of the stage
        //Note: Defining enums instead of passing pure integers everywhere makes the code more readable.
        var gatherObjectiveBeans  = new Objective(10, (int)ObjectiveType.Gather, 1);
        var gatherObjectiveShroom = new Objective(10, (int)ObjectiveType.Gather, 2);
        
        // If the quest has only one stage we can use the single-stage constructor of Quest class
        var stageDescription = "gathering 10 Beans and 10 mushrooms";
        var questTitle = "Gather Ingredients";
        
        //A quest with a single stage, containing the passed objectives, is created without creating a questStage object
        var quest = new Quest(1, questTitle,true,stageDescription,gatherObjectiveBeans,gatherObjectiveShroom);

        return quest;
    }
    

    // public static void Pro() {
    //     // Define some progress actions
    //     var killedWolf     = new ObjectiveProgressDto((int)ObjectiveType.Kill,1,3);
    //     var gatheredShroom = new ObjectiveProgressDto((int)ObjectiveType.Gather,1,2);
    //     
    //     // Play the game
    //     Console.WriteLine($"Quest {quest3.Id}: {quest3.Title}, Stages Left: {quest3.StagesLeft}");
    //
    //     Console.WriteLine("\nKilled a wolf");
    //     quest3.TryProgressQuest(killedWolf);
    //     PrintQuestStatus(quest3);
    //
    //     Console.WriteLine("\nGathered a mushroom");
    //     quest3.TryProgressQuest(gatheredShroom);
    //     PrintQuestStatus(quest3);
    //
    //     Console.WriteLine("\nGathered 8 mushrooms");
    //     for (int i = 0; i < 8; i++) { quest3.TryProgressQuest(gatheredShroom); }
    //     PrintQuestStatus(quest3);
    //
    //     Console.WriteLine("\nGathered 2 mushrooms");
    //     for (int i = 0; i < 2; i++) { quest3.TryProgressQuest(gatheredShroom); }
    //     PrintQuestStatus(quest3);
    // }
    
    //Print function
    /// <summary>
    /// Helper funstion to print the current status of a quest
    /// </summary>
    /// <param name="quest"></param>
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
}