using System.Collections.Generic;
using UnityEngine;

public class JobBoardSystem : MonoBehaviour
{
    public Transform contentRoot;
    public JobItemUI jobPrefab;

    public List<QuestSO> availableQuests;

    List<QuestSO> activeQuests = new List<QuestSO>();

    void OnEnable()
    {
        SpawnJobs();
    }

    void SpawnJobs()
    {
        foreach (Transform c in contentRoot)
            Destroy(c.gameObject);

        foreach (var quest in availableQuests)
        {
            var job = Instantiate(jobPrefab, contentRoot);
            job.Setup(quest, this);
        }
    }

    public void AcceptQuest(QuestSO quest)
    {
        QuestRuntimeSystem.Instance.AcceptQuest(quest);
        Debug.Log("da nhan quest");
        activeQuests.Add(quest);

        availableQuests.Remove(quest);

        SpawnJobs();
    }
}