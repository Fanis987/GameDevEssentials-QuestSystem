using QuestSystem.Entities;
using QuestSystem.Parser;
using QuestSystem.Parser.Util;

namespace QuestSystemTests.GodotExamples;

/// <summary>
/// Contains larger tests from godot examples
/// Contains larger tests from godot examples
/// </summary>
public class TestsFromGodot
{
    [Fact]
    public void ExampleFromGodot() {
        // First we declare the objectives of Stage 1 - Path 1:
        // 10 kills (action id = 3) of wolves ( asset/enemy id = 2)
        var killWolvesObjective = new Objective(10, (int)TaskType.Kill, (int)EnemyType.Wolf);
        // 1 Talk (action id = 2) to villager Bob (asset/NPC id = 1)
        var TalkBobObjective    = new Objective(1, (int)TaskType.Talk, (int)NpcType.VillagerBob);
    
        // Then we combine them in a stage path
        // Normal Path: ALL objectives must be completed to complete the path.
        // Selective Path: ANY of the objectives must be completed to complete the path.
        bool isSelective = false;
        
        // Each stage path indicates the stage unlocked, upon its completion
        // If completing this path ends the quest, set nextStageId to -1
        int nextStageId = 2; // in this case there is a next stage.
        StagePath path1_1 = new StagePath(isSelective, nextStageId, killWolvesObjective, TalkBobObjective);
        
        //We combine the stage paths to form stage 1
        int stageId = 1; //Should be unique per quest
        string stage1Description = "Kill 10 wolves AND talk to villager Bob";
        QuestStage stage1 = new QuestStage(stageId,stage1Description, path1_1);

        // Then we move to Stage 2 - Path 1:
        var talkGuardJackObjective = new Objective(1, (int)TaskType.Talk, (int)NpcType.GuardJack);
        StagePath path2_1 = new StagePath(false, -1, talkGuardJackObjective); // no next stage means '-1' in the 2nd arg
        string stage2Description = "Talk to guard Jack"; 
        QuestStage stage2 = new QuestStage(2,stage2Description, path2_1);
    
        //Finally, create the quest object
        int questId = 1; //Should be unique
        string questTitle = "Practicing the basics !";
        bool isMainQuest = true; // Optional: Common need to discriminate main and optional quests
        int nextQuestId = 2;     // Optional: For quest-chain support
        Quest newQuest = new Quest(questId, questTitle, isMainQuest, nextQuestId, stage1, stage2);
        
        Assert.False(newQuest.IsCompleted);
        
        newQuest.TryProgressQuest(5,(int)TaskType.Kill,(int)EnemyType.Wolf);
        Assert.False(newQuest.IsCompleted);
        
        newQuest.TryProgressQuest(5,(int)TaskType.Kill,(int)EnemyType.Wolf);
        Assert.False(newQuest.IsCompleted);
        Assert.NotNull(newQuest.CurrentStage);
        Assert.Equal(1,newQuest.CurrentStage.Id);
        
        newQuest.TryProgressQuest(1,(int)TaskType.Talk,(int)NpcType.VillagerBob);
        Assert.False(newQuest.IsCompleted);
        Assert.NotNull(newQuest.CurrentStage);
        Assert.Equal(2,newQuest.CurrentStage.Id);
        
        newQuest.TryProgressQuest(1,(int)TaskType.Talk,(int)NpcType.GuardJack);
        Assert.True(newQuest.IsCompleted);

    }

    [Fact]
    public void ExampleFromGodot2()
    {
        MultiParseResult parseResult = QuestParser.LoadFromJsonFile("GodotExamples/quest2.json");
        
        List<Quest> parsedQuestList = parseResult.Quests;
        var errorsList = parseResult.ErrorMessages;
        Assert.Single(parsedQuestList);
        
        var newQuest = parsedQuestList[0];
        Assert.False(newQuest.IsCompleted);
        
        newQuest.TryProgressQuest(5,(int)TaskType.Kill,(int)EnemyType.Wolf);
        Assert.False(newQuest.IsCompleted);
        
        newQuest.TryProgressQuest(5,(int)TaskType.Kill,(int)EnemyType.Wolf);
        Assert.False(newQuest.IsCompleted);
        Assert.NotNull(newQuest.CurrentStage);
        Assert.Equal(1,newQuest.CurrentStage.Id);
        
        newQuest.TryProgressQuest(1,(int)TaskType.Talk,(int)NpcType.VillagerBob);
        Assert.False(newQuest.IsCompleted);
        Assert.NotNull(newQuest.CurrentStage);
        Assert.Equal(2,newQuest.CurrentStage.Id);
        
        newQuest.TryProgressQuest(1,(int)TaskType.Talk,(int)NpcType.GuardJack);
        Assert.True(newQuest.IsCompleted);

    }
}