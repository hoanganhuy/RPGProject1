using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JobItemUI : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI giver;
    public TextMeshProUGUI from;
    public TextMeshProUGUI to;
    public TextMeshProUGUI desc;

    public Button acceptButton;

    QuestSO quest;
    JobBoardSystem board;

    public void Setup(QuestSO q, JobBoardSystem b)
    {
        quest = q;
        board = b;

        title.text = q.title;
        giver.text = "Giver: " + q.giverNPC;
        from.text = "From: " + q.receiveLocation.displayName;
        to.text = "To: " + q.steps[q.steps.Length - 1].location.displayName;
        desc.text = q.description;

        acceptButton.onClick.AddListener(OnAccept);
    }

    void OnAccept()
    {
        board.AcceptQuest(quest);
    }
}