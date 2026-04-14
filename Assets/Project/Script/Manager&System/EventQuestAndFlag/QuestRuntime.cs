
using UnityEngine;
public class QuestRuntime
{
    public string questId;
    public QuestSO data;

    public int currentStep;
    public int startHour;

    public bool completed;
    public bool expired;

    public QuestRuntime(QuestSO quest, int currentHour)
    {
        data = quest;
        if (quest == null)
        {
            Debug.LogError("QuestRuntime init with NULL quest!");
            return;
        }
        questId = quest.id;

        startHour = currentHour;
        currentStep = quest.startStep;
    }
}