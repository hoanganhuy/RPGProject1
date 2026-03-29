using UnityEngine;

public class QuestRuntimeSystem : MonoBehaviour
{
    public static QuestRuntimeSystem Instance;

    QuestRuntime activeQuest;

    void Awake()
    {
        Instance = this;
    }

    // ===== ACCEPT =====
    public bool HasActiveQuest()
    {
        return activeQuest != null;
    }

    public void AcceptQuest(QuestSO quest, int currentHour)
    {
        if (activeQuest != null)
        {
            Debug.Log("Already have active quest!");
            return;
        }

        activeQuest = new QuestRuntime(quest, currentHour);

        Debug.Log("Accepted quest: " + quest.title);
    }

    public QuestRuntime GetActiveQuest()
    {
        return activeQuest;
    }

    // ===== STEP =====
    public void AdvanceStep()
    {
        if (activeQuest == null)
            return;

        activeQuest.currentStep++;

        Debug.Log("Quest Step: " + activeQuest.currentStep);

        CheckCompletion();
    }

    // ===== COMPLETE =====
    void CheckCompletion()
    {
        if (activeQuest == null)
            return;

        if (activeQuest.currentStep >= activeQuest.data.endStep)
        {
            CompleteQuest();
        }
    }

    public void CompleteQuest()
    {
        Debug.Log("Quest Completed: " + activeQuest.data.title);

        FlagSystem.Instance.ClearFlag("quest_active_" + activeQuest.data.id);

        QuestHistorySystem.Instance.MarkCompleted(activeQuest.data.id);

        GameDataSystem.Instance.AddCoin(activeQuest.data.coinReward);
        GameDataSystem.Instance.AddRep(activeQuest.data.reputationReward);

        activeQuest = null;

        UIDataSystem.Instance.Refresh();
    }
}