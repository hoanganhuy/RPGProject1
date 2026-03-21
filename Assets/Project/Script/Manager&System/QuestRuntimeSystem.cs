using System.Collections.Generic;
using UnityEngine;

public class QuestRuntimeSystem : MonoBehaviour
{
    public static QuestRuntimeSystem Instance;

    List<QuestRuntime> activeQuests = new List<QuestRuntime>();

    void Awake()
    {
        Instance = this;
    }

    public void AcceptQuest(QuestSO quest)
    {
        activeQuests.Add(new QuestRuntime(quest));
    }

    public string GetDialogueForNPC(string npcID)
    {
        foreach (var q in activeQuests)
        {
            var step = q.GetCurrentStep();
            if (step != null && step.npcID == npcID)
                return step.stepDescription;
        }

        return "Hello traveler.";
    }

    public bool TryProgressQuest(string npcID)
    {
        foreach (var q in activeQuests)
        {
            var step = q.GetCurrentStep();

            if (step != null && step.npcID == npcID)
            {
                q.AdvanceStep();

                if (q.IsCompleted())
                {
                    CompleteQuest(q);
                    return true;
                }

                return false;
            }
        }

        return false;
    }

    void CompleteQuest(QuestRuntime q)
    {
        Debug.Log("Quest Complete: " + q.quest.title);

        //GameManager.Instance.AddCoins(q.quest.rewardCoin);
        //GameManager.Instance.AddRep(q.quest.rewardRep);

        activeQuests.Remove(q);
    }
}