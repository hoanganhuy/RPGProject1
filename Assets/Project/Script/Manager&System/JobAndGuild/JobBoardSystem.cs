using System.Collections.Generic;
using UnityEngine;

public class JobBoardSystem : MonoBehaviour
{
    public Transform contentRoot;
    public JobItemUI jobPrefab;
    public UIDataSystem dataSystem;

    //public List<QuestSO> availableQuests;
    public List<QuestSO> questPool;   // database quest
    List<QuestSO> todayBoard = new List<QuestSO>();
    void OnEnable()
    {        
        GenerateBoard();
        SpawnJobs();
    }

    public void SpawnJobs()
    {
        foreach (Transform c in contentRoot)
            Destroy(c.gameObject);

        foreach (var quest in todayBoard)
        {
            var job = Instantiate(jobPrefab, contentRoot);
            job.Setup(quest, this);
        }
    }
    bool CheckQuestCondition(QuestSO quest)
    {
        // ❗ chỉ 1 quest active
        if (QuestRuntimeSystem.Instance.HasActiveQuest())
            return false;

        // ❗ đã complete và không repeatable
        if (QuestHistorySystem.Instance.IsCompleted(quest.id)
            && !quest.repeatable)
            return false;

        return true;
    }
    public void GenerateBoard()
    {
        todayBoard.Clear();

        foreach (var q in questPool)
        {
            if (!CheckQuestCondition(q))
                continue;

            todayBoard.Add(q);

            if (todayBoard.Count >= 3)
                break;
        }
    }
    
    public void AcceptQuest(QuestSO quest)
    {
        if (QuestRuntimeSystem.Instance.HasActiveQuest())
            return;

        QuestRuntimeSystem.Instance.AcceptQuest(quest,10);

        //FlagSystem.Instance.SetFlag("quest_active_" + quest.id);

        todayBoard.Remove(quest);

        SpawnJobs();
    }
}