using QuestSystem;
using QuestSystem.Entities;

namespace ExampleUse;

/// <summary>
/// Offers some simple examples for quest creation and completion
/// </summary>
public static class Example {
    // Assume the following ids in our game:
    // Enemy ids: 1: toad 2: wolf
    // NPC ids: 1: TownGuard 2: Hunter
    // Item ids: 1: beans , 2: mushrooms
    
    /// <summary>
    /// A basic example of creating a quest with a single stage, the normal way
    /// The quest includes gathering 10 Beans and 10 mushrooms
    /// </summary>
    public static Quest CreateSingleStageQuest() {
        // First we define the objectives of the 1st stage
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
    /// A more complex example of creating a quest with the 3 following stages:
    /// Stage 1: Kill 5 toads and 3 wolves
    /// Stage 2: Talk to the town guard
    /// Stage 3: Kill 5 wolves OR talk to the hunter
    /// </summary>
    public static Quest CreateMultiStageQuest() {
        // First we define the objectives of the 1st stage
        //Note: Defining enums instead of passing pure integers everywhere makes the code more readable.
        var killObjectiveToads  = new Objective(5, (int)ObjectiveType.Kill, 1);
        var killObjectiveWolfs = new Objective(3, (int)ObjectiveType.Kill, 2);
        
        // Then we define the 1st stage that includes these objectives
        var firstStageDescription = "Kill 5 toads and 3 wolves";
        var firstStage = new QuestStageInclusive(firstStageDescription,killObjectiveToads, killObjectiveWolfs);
        
        // Then we define the 2nd stage and its objective
        var talkObjectiveGuard  = new Objective(1, (int)ObjectiveType.Talk, 1);
        var secondStageDescription = "Talk to the town guard";
        var secondStage = new QuestStageInclusive(secondStageDescription,talkObjectiveGuard);
        
        // Finally we define the 3rd stage and its objectives
        var killObjectiveWolfs2 = new Objective(5, (int)ObjectiveType.Kill, 2);
        var talkObjectiveHunter = new Objective(1, (int)ObjectiveType.Talk, 2);
        
        var thirdStageDescription = "Kill 5 wolves OR talk to the hunter";
        var thirdStage = new QuestStageSelective(thirdStageDescription,killObjectiveWolfs2,talkObjectiveHunter);
        
        // Finally we create the quest
        var questTitle = "Trouble in the forest";
        var quest = new Quest(2, questTitle , firstStage, secondStage, thirdStage);
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
    

    /// <summary>
    /// Completes the multi-stage quest  by a series of actions
    /// </summary>
    public static void CompleteMultiStageQuest(Quest quest) {
         // Define some progress actions
         var killedThreeToads  = new ObjectiveProgressDto((int)ObjectiveType.Kill,3,1);
         var killedOneWolf     = new ObjectiveProgressDto((int)ObjectiveType.Kill,1,2);
         
         var talkedToGuard  = new ObjectiveProgressDto((int)ObjectiveType.Talk,1,1);
         var talkedToHunter = new ObjectiveProgressDto((int)ObjectiveType.Talk,1,2);
         
         // Start doing some actions and see how the quest progresses
         Console.WriteLine($"Starting Quest {quest.Id}: '{quest.Title}', Stages Left: {quest.StagesLeft}");
    
         Console.WriteLine("\nKilled a wolf");
         quest.TryProgressQuest(killedOneWolf);
         PrintQuestStatus(quest);
         
         Console.WriteLine("\nKilled another wolf");
         quest.TryProgressQuest(killedOneWolf);
         PrintQuestStatus(quest);
         
         Console.WriteLine("\nKilled a 3rd wolf");
         quest.TryProgressQuest(killedOneWolf);
         PrintQuestStatus(quest);
         
         Console.WriteLine("\nKilled 3 toads");
         quest.TryProgressQuest(killedThreeToads);
         PrintQuestStatus(quest);
         
         Console.WriteLine("\nKilled 3 more toads, the first  stage finishes");
         quest.TryProgressQuest(killedThreeToads);
         PrintQuestStatus(quest);
         
         Console.WriteLine("\nKilled 3 more toads, no progress in the talk stage:");
         quest.TryProgressQuest(killedThreeToads);
         PrintQuestStatus(quest);
         
         Console.WriteLine("\nTalked to the hunter, no progress in the talk stage:");
         quest.TryProgressQuest(talkedToHunter);
         PrintQuestStatus(quest);
         
         Console.WriteLine("\nTalked to the town guard, the 2nd stage finishes");
         quest.TryProgressQuest(talkedToGuard);
         PrintQuestStatus(quest);
         
         Console.WriteLine("\nChose to talk to the hunter instead of killing wolves, quest will finish");
         quest.TryProgressQuest(talkedToHunter);
         PrintQuestStatus(quest);
     }
    
    /// <summary>
    /// Helper function to print the current status of a quest
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