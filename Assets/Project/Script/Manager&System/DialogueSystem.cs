using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public GameObject dialogPanel;

    public Image npcArt;
    public TextMeshProUGUI npcName;
    public TextMeshProUGUI description;

    public Button continueButton;

    string currentNPC;

    void Awake()
    {
        dialogPanel.SetActive(false);
        continueButton.onClick.AddListener(Continue);
    }

    public void StartDialogue(string npcID)
    {
        GameManager.Instance.ChangeMode(GameMode.InDialogue);

        currentNPC = npcID;

        dialogPanel.SetActive(true);

        npcName.text = npcID;
        description.text = QuestRuntimeSystem.Instance.GetDialogueForNPC(npcID);
    }

    void Continue()
    {
        bool questFinished = QuestRuntimeSystem.Instance.TryProgressQuest(currentNPC);

        if (questFinished)
        {
            description.text = "Quest Completed!";
            return;
        }

        dialogPanel.SetActive(false);
        GameManager.Instance.ChangeMode(GameMode.Exploration);
    }
}