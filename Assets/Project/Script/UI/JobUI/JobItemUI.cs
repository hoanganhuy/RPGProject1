using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JobItemUI : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI desc;
    public TextMeshProUGUI stepInfo;

    public Button acceptButton;

    QuestSO quest;
    JobBoardSystem board;

    public void Setup(QuestSO q, JobBoardSystem b)
    {
        quest = q;
        board = b;

        title.text = q.title;
        desc.text = q.description;

        stepInfo.text = $"Progress: {q.startStep} → {q.endStep}";

        acceptButton.onClick.RemoveAllListeners();
        acceptButton.onClick.AddListener(OnAccept);
    }

    void OnAccept()
    {
        if (QuestRuntimeSystem.Instance.HasActiveQuest())
            return;

        board.AcceptQuest(quest);
    }
}